using HarmonyLib;
using Microsoft.Xna.Framework;

// ReSharper disable InconsistentNaming

namespace Slothsoft.Challenger.Objects;

/// <summary>
/// This class patches <code>StardewValley.Debris</code>.
/// </summary>

static class MagicalDebris {
    internal static void PatchObject(Harmony harmony) {
        harmony.Patch(
            original: AccessTools.Method(
                typeof(Debris),
                nameof(Debris.collect),
                new[] {
                    typeof(Farmer),
                    typeof(Chunk),
                }),
            prefix: new(typeof(MagicalDebris), nameof(Collect))
        );
    }

    private static bool Collect(Debris __instance, ref bool __result, Farmer farmer, Chunk? chunk = null) {
        if (__instance.chunkType.Value == -MagicalObject.ObjectId) {
            __result = farmer.addItemToInventoryBool(new SObject(Vector2.Zero, MagicalObject.ObjectId));
            return false;
        }
        return true;
    }
}