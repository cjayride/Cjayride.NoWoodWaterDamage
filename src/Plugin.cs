using System.Runtime.CompilerServices;
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
        public const string PluginVersion = "1.1.2";

        public static ConfigEntry<bool> PreventAllWeatherDamage { get; private set; }
        public static ConfigEntry<bool> ProtectAtWaterline { get; private set; }

        internal static bool PreventAll;
        internal static bool ProtectWaterline;

        private void Awake()
        {
            PreventAllWeatherDamage = Config.Bind(
                "General", "PreventAllWeatherDamage", true,
                "When enabled, ALL wood/hardwood/timberwood pieces are immune to rain and weather damage everywhere, not just at the waterline. Set to false to only protect at the waterline.");

            ProtectAtWaterline = Config.Bind(
                "General", "ProtectAtWaterline", true,
                "Protect wood pieces at the waterline (docks, shore). Leave this true even if PreventAllWeatherDamage is true.");

            PreventAll = PreventAllWeatherDamage.Value;
            ProtectWaterline = ProtectAtWaterline.Value;
            PreventAllWeatherDamage.SettingChanged += (_, __) => PreventAll = PreventAllWeatherDamage.Value;
            ProtectAtWaterline.SettingChanged += (_, __) => ProtectWaterline = ProtectAtWaterline.Value;

            Config.Save();

            new Harmony(PluginGuid).PatchAll();

            if (PreventAll)
                Logger.LogInfo("All wood/hardwood/timberwood pieces are fully immune to rain and weather damage everywhere.");
            else if (ProtectWaterline)
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
        private const float WaterlineMargin = 2.5f;

        private static readonly ConditionalWeakTable<WearNTear, object> ShipLookup = new ConditionalWeakTable<WearNTear, object>();
        private static readonly object ShipYes = new object();
        private static readonly object ShipNo = new object();

        /// <summary>
        /// Default config skips vanilla IsWet (including Floating.IsUnderWater) for protected wood.
        /// Ships keep vanilla behaviour.
        /// </summary>
        private static bool Prefix(WearNTear __instance, ref bool __result)
        {
            if (__instance == null)
                return true;

            if (!Plugin.PreventAll && !Plugin.ProtectWaterline)
                return true;

            WearNTear.MaterialType material = __instance.m_materialType;
            if (material != WearNTear.MaterialType.Wood
                && material != WearNTear.MaterialType.HardWood
                && material != WearNTear.MaterialType.Timberwood)
                return true;

            if (IsOnShip(__instance))
                return true;

            if (Plugin.PreventAll)
            {
                __result = false;
                return false;
            }

            return true;
        }

        private static void Postfix(WearNTear __instance, ref bool __result)
        {
            if (!__result || __instance == null || Plugin.PreventAll || !Plugin.ProtectWaterline)
                return;

            WearNTear.MaterialType material = __instance.m_materialType;
            if (material != WearNTear.MaterialType.Wood
                && material != WearNTear.MaterialType.HardWood
                && material != WearNTear.MaterialType.Timberwood)
                return;

            if (IsOnShip(__instance))
                return;

            if (IsAtWaterline(__instance))
                __result = false;
        }

        private static bool IsOnShip(WearNTear wear)
        {
            object cached;
            if (ShipLookup.TryGetValue(wear, out cached))
                return cached == ShipYes;

            bool onShip = false;
            Transform transform = wear.transform;
            if (transform != null)
            {
                Transform root = transform.root;
                onShip = root != null && root.GetComponent<Ship>() != null;
            }

            ShipLookup.Add(wear, onShip ? ShipYes : ShipNo);
            return onShip;
        }

        private static bool IsAtWaterline(WearNTear piece)
        {
            Transform transform = piece.transform;
            if (transform == null)
                return false;

            Vector3 pos = transform.position;
            WaterVolume volume = null;
            float surface = Floating.GetWaterLevel(pos, ref volume);
            if (surface < -1000f)
                return false;

            return pos.y <= surface + WaterlineMargin;
        }
    }
}
