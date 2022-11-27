using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Netcode;
using StardewValley.Network;

// ReSharper disable InconsistentNaming

namespace Slothsoft.Challenger.Objects;

/// <summary>
/// This class patches <code>StardewValley.GameLocation</code>.
/// </summary>
internal static class MagicalGameLocation {
    private static readonly HashSet<Debris> MagicalObjectDebris = new();

    internal static void PatchObject(Harmony harmony) {
        harmony.Patch(
            original: AccessTools.Method(
                typeof(GameLocation),
                "drawDebris",
                new[] {
                    typeof(SpriteBatch),
                }),
            prefix: new HarmonyMethod(typeof(MagicalGameLocation), nameof(RemoveMagicalObjectDebris)),
            postfix: new HarmonyMethod(typeof(MagicalGameLocation), nameof(DrawAndReAddMagicalObjectDebris))
        );
    }

    /// <summary>
    /// Since the normal method cannot draw our final magical objects, we need to remove them and draw them later.
    /// </summary>
    private static void RemoveMagicalObjectDebris(GameLocation __instance, SpriteBatch b) {
        MagicalObjectDebris.Clear();

        if (!ChallengerMod.Instance.IsInitialized()) {
            // something went really wrong if we are here
            return;
        }

        if (!ChallengerMod.Instance.GetApi()!.IsActiveChallengeCompleted) {
            // we can draw everything but the final object
            return;
        }

        var magicalObjectDebrises = __instance.debris
            .Where(d => d != null)
            .Select(d => d!)
            .Where(d => d.chunkType.Value == -MagicalObject.ObjectId)
            .ToArray();
        foreach (var magicalObjectDebris in magicalObjectDebrises) {
            MagicalObjectDebris.Add(magicalObjectDebris);
            __instance.debris.Remove(magicalObjectDebris);
        }
    }

    /// <summary>
    /// Since the normal method cannot draw our magical objects, we need to remove them and draw them later.
    /// </summary>
    private static void DrawAndReAddMagicalObjectDebris(GameLocation __instance, SpriteBatch b) {
        if (MagicalObjectDebris.Count == 0) {
            return;
        }

        DrawMagicalObjectDebris(b);

        foreach (var magicalObjectDebris in MagicalObjectDebris) {
            __instance.debris.Add(magicalObjectDebris);
        }

        MagicalObjectDebris.Clear();
    }

    private static void DrawMagicalObjectDebris(SpriteBatch b) {
        foreach (var debri in MagicalObjectDebris) {
            foreach (var t in debri.Chunks) {
                if (t.debrisType <= 0) {
                    b.Draw(MagicalObject._finalMagicalObject,
                        Utility.snapDrawPosition(Game1.GlobalToLocal(Game1.viewport,
                            (Vector2) (NetPausableField<Vector2, NetVector2, NetVector2>) t.position + new Vector2(32f, 64f))),
                        null, Color.White, 0.0f, new Vector2(8f, 32f), 3.2f, SpriteEffects.None,
                        (float) ((debri.chunkFinalYLevel + 48 + t.position.X / 10000.0) / 10000.0));
                    var spriteBatch = b;
                    var shadowTexture = Game1.shadowTexture;
                    var position = Utility.snapDrawPosition(Game1.GlobalToLocal(Game1.viewport,
                        new Vector2(t.position.X + 25.6f,
                            (float) ((debri.chunksMoveTowardPlayer ? t.position.Y + 8.0 : debri.chunkFinalYLevel) +
                                     32.0))));
                    Rectangle? sourceRectangle = Game1.shadowTexture.Bounds;
                    var white = Color.White;
                    var bounds = Game1.shadowTexture.Bounds;
                    var x = (double) bounds.Center.X;
                    var y = (double) bounds.Center.Y;
                    var origin = new Vector2((float) x, (float) y);
                    double scale = Math.Min(3f,
                        (float) (3.0 -
                                 (debri.chunksMoveTowardPlayer ? 0.0 : (debri.chunkFinalYLevel - (double) t.position.Y) / 128.0)));
                    var layerDepth = debri.chunkFinalYLevel / 10000.0;
                    spriteBatch.Draw(shadowTexture, position, sourceRectangle, white, 0.0f, origin, (float) scale, SpriteEffects.None, (float) layerDepth);
                }
            }
        }
    }
}