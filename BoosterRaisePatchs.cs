using BoosterRaise.Common.Logging;
using HarmonyLib;
using Microsoft.Extensions.Logging;

namespace BoosterRaise;

public static class BoosterRaisePatchs
{
    private static readonly ILogger logger = LoggerFactory.CreateLogger(nameof(BoosterRaisePatchs));

    [HarmonyPatch(typeof(DropServerManager), nameof(DropServerManager.NewGameSession))]
    [HarmonyPrefix]
    public static void NewGameSession(DropServerManager __instance, string sessionId, ref uint[]? boosterIds)
    {
        logger.LogDebug($"Patch NewGameSession, sessionId: {sessionId}, boosters: [{boosterIds.Join()}]");

        boosterIds = null;
    }

    [HarmonyPatch(typeof(DropServerGameSession), nameof(DropServerGameSession.ConsumeBoosters))]
    [HarmonyPrefix]
    public static bool ConsumeBoosters()
    {
        logger.LogDebug("Patch ConsumeBoosters, skip execute");

        return false;
    }
}