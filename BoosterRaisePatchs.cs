using HarmonyLib;

namespace BoosterRaise;

public static class BoosterRaisePatchs {

    [HarmonyPatch(typeof(DropServerManager), nameof(DropServerManager.NewGameSession))]
    [HarmonyPrefix]
    public static void NewGameSession(DropServerManager __instance, string sessionId, ref uint[]? boosterIds)
    {
    }
}