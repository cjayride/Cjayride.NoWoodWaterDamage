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
        public const string PluginVersion = "1.0.0";

        private void Awake()
        {
            new Harmony(PluginGuid).PatchAll();
            Logger.LogInfo("Wood, hardwood, and timberwood pieces no longer wear from being in water. Rain on an unroofed piece still does.");
        }
    }

    /// <summary>
    /// WearNTear treats rain and submersion as the same wet state, then chips 5% health
    /// every 60 seconds while the piece is above half health. Docks sit in that state
    /// even in clear weather. Rain on land is left alone. Ships keep vanilla wear.
    /// The piece owner runs this, so both the client and the dedicated server need it.
    /// </summary>
    [HarmonyPatch(typeof(WearNTear), nameof(WearNTear.IsWet))]
    internal static class SkipSubmergedWood
    {
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

            if (__instance.IsUnderWater())
                __result = false;
        }
    }
}
