using BepInEx;
using HarmonyLib;
using UnityEngine;

namespace Cjayride.NoWoodWaterDamage
{
    [BepInPlugin(PluginGuid, PluginName, PluginVersion)]
    public class Plugin : BaseUnityPlugin
    {
        public const string PluginGuid = "cjayride.NoWoodWaterDamage";
        public const string PluginName = "No Wood Water Damage";
        public const string PluginVersion = "1.0.1";

        private void Awake()
        {
            new Harmony(PluginGuid).PatchAll();
            Logger.LogInfo("Wood, hardwood, and timberwood pieces no longer wear from water or rain while at the waterline (docks). Rain on dry land is unchanged.");
        }
    }

    /// <summary>
    /// WearNTear treats rain and submersion as the same wet state, then chips 5% health
    /// every 60 seconds while the piece is above half health and unroofed.
    /// <see cref="WearNTear.IsUnderWater"/> is only true when the piece origin is below the
    /// surface, so dock planks at the waterline still counted as wet from rain. Ships keep vanilla wear.
    /// The piece owner runs this, so both the client and the dedicated server need it.
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

            if (__instance.GetComponentInParent<Ship>() != null)
                return;

            if (__instance.IsUnderWater() || IsAtWaterline(__instance))
                __result = false;
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
