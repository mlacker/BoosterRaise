using BoosterImplants;
using BoosterRaise.Common.Logging;
using GameData;
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

    [HarmonyPatch(typeof(ArtifactInventory), nameof(ArtifactInventory.GetArtifactCount))]
    [HarmonyPrefix]
    public static bool GetArtifactCount(ref int __result, ArtifactCategory category)
    {
        __result = 200;

        logger.LogDebug($"Patch GetArtifactCount, skip execute and category {category} return {__result}");

        return false;
    }
}