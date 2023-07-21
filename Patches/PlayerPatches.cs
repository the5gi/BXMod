using HarmonyLib;
using GorillaLocomotion;
using System;
using BXMod.Modules.Physics;
using BXMod.Tools;

namespace BXMod.Patches
{
    [HarmonyPatch(typeof(Player))]
    [HarmonyPatch("LateUpdate", MethodType.Normal)]
    public class LateUpdatePatch
    {
        public static Action<Player> OnLateUpdate;
        private static void Postfix(Player __instance)
        {
            try
            {
                OnLateUpdate?.Invoke(__instance);
            }
            catch(Exception e) { Logging.Exception(e); }
        }
    }

    [HarmonyPatch(typeof(Player))]
    [HarmonyPatch("GetSlidePercentage", MethodType.Normal)]
    public class SlidePatch
    {
        private static void Postfix(Player __instance, ref float __result)
        {
            if(SlipperyHands.Instance)
                    __result = SlipperyHands.Instance.enabled ? 1 : __result;
            if(NoSlip.Instance)
                __result = NoSlip.Instance.enabled ? 0 : __result;
        }
    }
}
