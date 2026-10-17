using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using UnityEngine;

namespace Cjayride.NoWoodWaterDamage
{
    [BepInPlugin(PluginGuid, PluginName, PluginVersion)]
    public class Plugin : BaseUnityPlugin
    {
        public const string PluginGuid = "cjayride.NoWoodWaterDamage";
        public const string PluginName = "No Wood Water Damage";
        public const string PluginVersion = "1.1.0";

        public static ConfigEntry<bool> PreventAllWeatherDamage { get; private set; }
        public static ConfigEntry<bool> ProtectAtWaterline { get; private set; }

        private void Awake()
        {
            // By default the mod prevents all water related damage (rain, snow, weather, seasons).
            PreventAllWeatherDamage = Config.Bind(
                "General", "PreventAllWeatherDamage", true,
                "When enabled, ALL wood/hardwood/timberwood pieces are immune to rain and weather damage everywhere, not just at the waterline. Set to false to only protect at the waterline.");

            // Existing behavior - protect pieces sitting at or below water surface
            ProtectAtWaterline = Config.Bind(
                "General", "ProtectAtWaterline", true,
                "Protect wood pieces at the waterline (docks, shore). Leave this true even if PreventAllWeatherDamage is true.");

            Config.Save();

            new Harmony(PluginGuid).PatchAll();

            if (PreventAllWeatherDamage.Value)
                Logger.LogInfo("All wood/hardwood/timberwood pieces are fully immune to rain and weather damage everywhere.");
            else if (ProtectAtWaterline.Value)
                Logger.LogInfo("Wood protection limited to the waterline only. Rain on dry land still causes wear.");
            else
                Logger.LogWarning("All wood protection disabled in config. Wood will take normal weather damage.");
        }
    }

    /// <summary>
    /// WearNTear treats rain and submersion as the same wet state, then chips 5% health
    /// every 60 seconds while the piece is above half health and unroofed.
    /// Both the client and the dedicated server need this mod for full protection.
    /// </summary>
    [HarmonyPatch(typeof(WearNTear), nameof(WearNTear.IsWet))]
    internal static class SkipSubmergedWood
    {
        /// <summary>How far above the local water surface a wood piece can sit and still count as "at the waterline".</summary>
        private const float WaterlineMargin = 2.5f;

        private static void Postfix(WearNTear __instance, ref bool __result)
        {
            if (!__result || __instance == null)
                return;

            WearNTear.MaterialType material = __instance.m_materialType;
            if (material != WearNTear.MaterialType.Wood
                && material != WearNTear.MaterialType.HardWood
                && material != WearNTear.MaterialType.Timberwood)
                return;

            // Ships always keep vanilla wear behaviour
            if (__instance.GetComponentInParent<Ship>() != null)
                return;

            // If full protection is enabled, block ALL weather damage immediately
            if (Plugin.PreventAllWeatherDamage.Value)
            {
                __result = false;
                return;
            }

            // Fallback: only protect pieces at or below the waterline
            if (Plugin.ProtectAtWaterline.Value)
            {
                if (IsAtWaterline(__instance))
                    __result = false;
            }
        }

        private static bool IsAtWaterline(WearNTear piece)
        {
            Transform transform = piece.transform;
            if (transform == null)
                return false;

            Vector3 pos = transform.position;
            WaterVolume volume = null;
            float surface = Floating.GetWaterLevel(pos, ref volume);
            // Floating uses a large negative sentinel when there is no water here.
            if (surface < -1000f)
                return false;

            return pos.y <= surface + WaterlineMargin;
        }
    }
}
