using System;
using System.Collections.Generic;
using HarmonyLib;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Slothsoft.Challenger.Api;
using Slothsoft.Challenger.Common;
using Slothsoft.Challenger.Menus;
using StardewModdingAPI.Events;

// ReSharper disable InconsistentNaming

namespace Slothsoft.Challenger.Objects;

/// <summary>
/// This class patches <code>StardewValley.Object</code>.
/// </summary>
public static class MagicalObject {
    public const int ObjectId = 570745037;
    internal const string ObjectName = "Magical Object";
    private const bool ObjectBigCraftable = true;

    private static readonly HashSet<SObject> MagicalObjects = new();
    internal static Texture2D? _finalMagicalObject;

    internal static void PatchObject(string uniqueId) {
        var helper = ChallengerMod.Instance.Helper;
        helper.Events.Content.AssetRequested += OnAssetRequested;
        helper.Events.Input.ButtonPressed += OnButtonPressed;
        _finalMagicalObject ??= helper.ModContent.Load<Texture2D>("assets/magical_object.png");

        var harmony = new Harmony(uniqueId);

        MagicalDebris.PatchObject(harmony);
        MagicalGame1.PatchObject(harmony);
        MagicalWorldState.PatchObject(harmony);
        MagicalGameLocation.PatchObject(harmony);

        harmony.Patch(
            original: AccessTools.Method(
                typeof(SObject),
                nameof(SObject.draw),
                new[] {
                    typeof(SpriteBatch), // spriteBatch
                    typeof(int), // x
                    typeof(int), // y
                    typeof(float), // alpha
                }),
            prefix: new HarmonyMethod(typeof(MagicalObject), nameof(DrawOrMakeVanillaObject)),
            postfix: new HarmonyMethod(typeof(MagicalObject), nameof(MakeMagicalObject))
        );
        harmony.Patch(
            original: AccessTools.Method(
                typeof(SObject),
                nameof(SObject.draw),
                new[] {
                    typeof(SpriteBatch), // spriteBatch
                    typeof(int), // xNonTile
                    typeof(int), // yNonTile
                    typeof(float), // layerDepth
                    typeof(float), // alpha
                }),
            prefix: new HarmonyMethod(typeof(MagicalObject), nameof(DrawNoneTileOrMakeVanillaObject)),
            postfix: new HarmonyMethod(typeof(MagicalObject), nameof(MakeMagicalObject))
        );
        harmony.Patch(
            original: AccessTools.Method(
                typeof(SObject),
                nameof(SObject.drawAsProp),
                new[] {
                    typeof(SpriteBatch), // b
                }),
            prefix: new HarmonyMethod(typeof(MagicalObject), nameof(DrawAsPropOrMakeVanillaObject)),
            postfix: new HarmonyMethod(typeof(MagicalObject), nameof(MakeMagicalObject))
        );
        harmony.Patch(
            original: AccessTools.Method(
                typeof(SObject),
                nameof(SObject.drawInMenu),
                new[] {
                    typeof(SpriteBatch), // spriteBatch
                    typeof(Vector2), // location
                    typeof(float), // scaleSize
                    typeof(float),
                    typeof(float),
                    typeof(StackDrawType),
                    typeof(Color),
                    typeof(bool),
                }),
            prefix: new HarmonyMethod(typeof(MagicalObject), nameof(DrawInMenuOrMakeVanillaObject)),
            postfix: new HarmonyMethod(typeof(MagicalObject), nameof(MakeMagicalObject))
        );
        harmony.Patch(
            original: AccessTools.Method(
                typeof(SObject),
                nameof(SObject.drawWhenHeld),
                new[] {
                    typeof(SpriteBatch), // spriteBatch
                    typeof(Vector2), // objectPosition
                    typeof(Farmer),
                }),
            prefix: new HarmonyMethod(typeof(MagicalObject), nameof(DrawWhenHeldOrMakeVanillaObject)),
            postfix: new HarmonyMethod(typeof(MagicalObject), nameof(MakeMagicalObject))
        );

        harmony.Patch(
            original: AccessTools.Method(
                typeof(SObject),
                nameof(SObject.isActionable),
                new[] {
                    typeof(Farmer), // who
                }),
            prefix: new HarmonyMethod(typeof(MagicalObject), nameof(MakeVanillaObject)),
            postfix: new HarmonyMethod(typeof(MagicalObject), nameof(MakeMagicalObject))
        );
    }

    private static void OnAssetRequested(object? sender, AssetRequestedEventArgs e) {
        if (e.Name.StartsWith("Data/BigCraftablesInformation")) {
            // See documentation: https://stardewvalleywiki.com/Modding:Items#Big_craftables
            // name / price / edibility / type and category / description / outdoors / indoors / fragility / is lamp / display name
            e.Edit(
                asset => {
                    var helper = ChallengerMod.Instance.Helper;
                    var description = helper.Translation.Get("MagicalObject.Description");
                    var name = helper.Translation.Get("MagicalObject.Name");

                    var data = asset.AsDictionary<int, string>().Data;
                    data.Add(
                        ObjectId,
                        $"{ObjectName}/0/-300/Crafting -9/{description}/true/true/0//{name}"
                    );
                });
        }
    }

    private static void OnButtonPressed(object? sender, ButtonPressedEventArgs e) {
        // we don't interact with anything here
        if (!Context.IsPlayerFree) {
            return;
        }

        // Filter out all objects that are not magical or still in use
        var pos = CommonHelpers.GetCursorTile(1);
        if (!Game1.currentLocation.Objects.TryGetValue(pos, out var obj)
            || !IsMagicalObject(obj)
            || obj.heldObject.Value is not null
            || obj.MinutesUntilReady > 0) {
            return;
        }

        // we have a magical object on our hands

        if (ChallengerMod.Instance.GetApi()!.IsActiveChallengeCompleted) {
            // we have the final iteration of the magical object
            OnButtonPressedOnFinalMagicalObject(obj);
            return;
        } 
        
        try {
            MakeVanillaObject(obj);
            if (obj.ParentSheetIndex == MagicalReplacement.Default.ParentSheetIndex) {
                if (!e.Button.IsUseToolButton()) {
                    // we have no special item for this challenge -> open challenge Menu
                    if (Game1.activeClickableMenu == null && (e.Button.IsActionButton() || e.Button.IsUseToolButton()))
                        Game1.activeClickableMenu = new ChallengeMenu();
                }
            } else if (e.Button.IsUseToolButton() && Game1.player.CurrentItem != null) {
                if (Game1.player.CurrentItem.getOne() is SObject heldItem && obj.performObjectDropInAction(heldItem, false, Game1.player)) {
                    Game1.player.reduceActiveItemByOne();
                }
            }
        } finally {
            MakeMagicalObject(obj);
        }
    }
    
    private static void OnButtonPressedOnFinalMagicalObject(SObject obj)
    {
        obj.heldObject.Value = (SObject)Game1.player.CurrentItem.getOne();
        Game1.currentLocation.playSound("select");
        obj.MinutesUntilReady = 1;
        Game1.player.reduceActiveItemByOne();
    }

    private static bool DrawOrMakeVanillaObject(SObject __instance, SpriteBatch spriteBatch, int x, int y) {
        if (!IsMagicalObject(__instance) || !ChallengerMod.Instance.IsInitialized())
            return true;

        if (ChallengerMod.Instance.GetApi()!.IsActiveChallengeCompleted) {
            // make another object out of the magical object    
            Draw(__instance, spriteBatch, x, y);
            return false;
        }

        MakeVanillaObject(__instance);
        return true;
    }

    private static void Draw(SObject instance, SpriteBatch spriteBatch, float x, float y) {
        if (instance.isTemporarilyInvisible)
            return;

        var vector2 = instance.getScale() * 4f;
        var local = Game1.GlobalToLocal(Game1.viewport, new Vector2(x * Game1.tileSize, y * Game1.tileSize - Game1.tileSize));
        var destinationRectangle = new Rectangle(
            (int) (local.X - vector2.X / 2.0) + (instance.shakeTimer > 0 ? Game1.random.Next(-1, 2) : 0),
            (int) (local.Y - vector2.Y / 2.0) + (instance.shakeTimer > 0 ? Game1.random.Next(-1, 2) : 0),
            (int) (Game1.tileSize + vector2.X),
            (int) (2 * Game1.tileSize + vector2.Y / 2.0)
        );
        var layerDepth = Math.Max(0.0f, ((y + 1) * Game1.tileSize - 24) / 10000f) + x * 1E-05f;

        spriteBatch.Draw(_finalMagicalObject, destinationRectangle, null, Color.White, 0.0f, Vector2.Zero, SpriteEffects.None, layerDepth);
    }

    private static bool DrawNoneTileOrMakeVanillaObject(SObject __instance, SpriteBatch spriteBatch, int xNonTile, int yNonTile, float layerDepth,
        float alpha = 1f) {
        if (!IsMagicalObject(__instance) || !ChallengerMod.Instance.IsInitialized())
            return true;

        if (ChallengerMod.Instance.GetApi()!.IsActiveChallengeCompleted) {
            // make another object out of the magical object    
            DrawNoneTile(__instance, spriteBatch, xNonTile, yNonTile, layerDepth, alpha);
            return false;
        }

        MakeVanillaObject(__instance);
        return true;
    }

    private static void DrawNoneTile(SObject instance, SpriteBatch spriteBatch, int xNonTile, int yNonTile, float layerDepth, float alpha = 1f) {
        var vector2 = instance.getScale() * 4f;
        var local = Game1.GlobalToLocal(Game1.viewport, new Vector2(xNonTile, yNonTile));
        var destinationRectangle = new Rectangle(
            (int) (local.X - vector2.X / 2.0) + (instance.shakeTimer > 0 ? Game1.random.Next(-1, 2) : 0),
            (int) (local.Y - vector2.Y / 2.0) + (instance.shakeTimer > 0 ? Game1.random.Next(-1, 2) : 0),
            (int) (Game1.tileSize + vector2.X),
            (int) (2 * Game1.tileSize + vector2.Y / 2.0)
        );
        spriteBatch.Draw(_finalMagicalObject, destinationRectangle, null, Color.White * alpha, 0.0f, Vector2.Zero, SpriteEffects.None, layerDepth);
    }

    private static bool DrawAsPropOrMakeVanillaObject(SObject __instance, SpriteBatch b) {
        if (!IsMagicalObject(__instance) || !ChallengerMod.Instance.IsInitialized())
            return true;

        if (ChallengerMod.Instance.GetApi()!.IsActiveChallengeCompleted) {
            // make another object out of the magical object    
            DrawAsProp(__instance, b);
            return false;
        }

        MakeVanillaObject(__instance);
        return true;
    }

    private static void DrawAsProp(SObject instance, SpriteBatch b) {
        var vector = instance.getScale() * 4f;
        var local = Game1.GlobalToLocal(Game1.viewport, new Vector2(
            (int) instance.TileLocation.X * Game1.tileSize,
            (int) instance.TileLocation.Y * Game1.tileSize - Game1.tileSize)
        );
        var destinationRectangle = new Rectangle(
            (int) (local.X - vector.X / 2.0),
            (int) (local.Y - vector.Y / 2.0),
            (int) (Game1.tileSize + vector.X),
            (int) (2 * Game1.tileSize + vector.Y / 2.0)
        );
        b.Draw(_finalMagicalObject, destinationRectangle, null, Color.White, 0.0f, Vector2.Zero, SpriteEffects.None,
            Math.Max(0.0f, ((instance.TileLocation.Y + 1) * Game1.tileSize - 1) / 10000f));
    }

    private static bool DrawInMenuOrMakeVanillaObject(SObject __instance, SpriteBatch spriteBatch, Vector2 location, float scaleSize, float transparency,
        float layerDepth, StackDrawType drawStackNumber, Color color, bool drawShadow) {
        if (!IsMagicalObject(__instance) || !ChallengerMod.Instance.IsInitialized())
            return true;

        if (ChallengerMod.Instance.GetApi()!.IsActiveChallengeCompleted) {
            // make another object out of the magical object    
            DrawInMenu(__instance, spriteBatch, location, scaleSize, transparency, layerDepth, drawStackNumber, color, drawShadow);
            return false;
        }

        MakeVanillaObject(__instance);
        return true;
    }

    private static void DrawInMenu(SObject instance, SpriteBatch spriteBatch, Vector2 location, float scaleSize, float transparency, float layerDepth,
        StackDrawType drawStackNumber, Color color, bool drawShadow) {
        var flag = (drawStackNumber == StackDrawType.Draw && instance.maximumStackSize() > 1 && instance.Stack > 1 ||
                    drawStackNumber == StackDrawType.Draw_OneInclusive) && scaleSize > 0.3 && instance.Stack != int.MaxValue;
        spriteBatch.Draw(_finalMagicalObject, location + new Vector2(32f, 32f), null, color * transparency, 0.0f, new Vector2(8f, 16f),
            (float) (4.0 * (scaleSize < 0.2 ? scaleSize : scaleSize / 2.0)), SpriteEffects.None, layerDepth);
        if (flag)
            Utility.drawTinyDigits(instance.Stack, spriteBatch,
                location + new Vector2(Game1.tileSize - Utility.getWidthOfTinyDigitString(instance.Stack, 3f * scaleSize) + 3f * scaleSize,
                    (float) (Game1.tileSize - 18.0 * scaleSize + 2.0)), 3f * scaleSize, 1f, color);
    }

    private static bool DrawWhenHeldOrMakeVanillaObject(SObject __instance, SpriteBatch spriteBatch, Vector2 objectPosition, Farmer f) {
        if (!IsMagicalObject(__instance) || !ChallengerMod.Instance.IsInitialized())
            return true;

        if (ChallengerMod.Instance.GetApi()!.IsActiveChallengeCompleted) {
            // make another object out of the magical object    
            DrawWhenHeld(__instance, spriteBatch, objectPosition, f);
            return false;
        }

        MakeVanillaObject(__instance);
        return true;
    }

    private static void DrawWhenHeld(SObject instance, SpriteBatch spriteBatch, Vector2 objectPosition, Farmer f) {
        spriteBatch.Draw(_finalMagicalObject, objectPosition, null, Color.White, 0.0f, Vector2.Zero, 4f,
            SpriteEffects.None, Math.Max(0.0f, (f.getStandingY() + 3) / 10000f));
    }

    private static void MakeVanillaObject(SObject __instance) {
        if (!IsMagicalObject(__instance) || !ChallengerMod.Instance.IsInitialized())
            return;

        MagicalObjects.Add(__instance);

        var magicalReplacement = ChallengerMod.Instance.GetApi()!.ActiveChallengeMagicalReplacement;
        __instance.ParentSheetIndex = magicalReplacement.ParentSheetIndex;
        __instance.Name = magicalReplacement.Name;
    }

    internal static bool IsMagicalObject(SObject instance) {
        return instance is {bigCraftable.Value: ObjectBigCraftable, ParentSheetIndex: ObjectId};
    }

    private static void MakeMagicalObject(SObject __instance) {
        if (!MagicalObjects.Contains(__instance))
            return;

        __instance.ParentSheetIndex = ObjectId;
        __instance.Name = ObjectName;
    }
}