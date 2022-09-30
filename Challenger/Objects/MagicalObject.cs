using HarmonyLib;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Slothsoft.Challenger.Models;
using StardewModdingAPI.Events;
using StardewValley.Objects;
// ReSharper disable InconsistentNaming

namespace Slothsoft.Challenger.Objects;

public static class MagicalObject {
    
    internal const int ObjectId = 570745037;
    internal const string ObjectName = "Magical Object";
    private static readonly SObject DefaultAppearance = new Furniture(ObjectIndexes.LeahsSculpture, Vector2.Zero);

    public static void PatchObject(string uniqueId) {
        var helper = ChallengerMod.Instance.Helper;
        helper.Events.Content.AssetRequested += OnAssetRequested;
        
        var harmony = new Harmony(uniqueId);
        harmony.Patch(
            original: AccessTools.Method(
                typeof(SObject),
                nameof(SObject.draw),
                new[] {
                    typeof(SpriteBatch),
                    typeof(int),
                    typeof(int),
                    typeof(float),
                }),
            prefix: new(typeof(MagicalObject), nameof(Draw))
        );
        harmony.Patch(
            original: AccessTools.Method(
                typeof(SObject),
                nameof(SObject.draw),
                new[] {
                    typeof(SpriteBatch),
                    typeof(int),
                    typeof(int),
                    typeof(float),
                    typeof(float),
                }),
            prefix: new(typeof(MagicalObject), nameof(DrawWithAlpha))
        );
        harmony.Patch(
            original: AccessTools.Method(
                typeof(SObject),
                nameof(SObject.drawAsProp),
                new[] {
                    typeof(SpriteBatch),
                }),
            prefix: new(typeof(MagicalObject), nameof(DrawAsProp))
        );
        harmony.Patch(
            original: AccessTools.Method(
                typeof(SObject),
                nameof(SObject.drawInMenu),
                new[] {
                    typeof(SpriteBatch),
                    typeof(Vector2),
                    typeof(float),
                    typeof(float),
                    typeof(float),
                    typeof(StackDrawType),
                    typeof(Color),
                    typeof(bool),
                }),
            prefix: new(typeof(MagicalObject), nameof(DrawInMenu))
        );
        harmony.Patch(
            original: AccessTools.Method(
                typeof(SObject),
                nameof(SObject.drawWhenHeld),
                new[] {
                    typeof(SpriteBatch),
                    typeof(Vector2),
                    typeof(Farmer),
                }),
            prefix: new(typeof(MagicalObject), nameof(DrawWhenHeld))
        );
    }
    
    private static void OnAssetRequested(object? sender, AssetRequestedEventArgs e) {
        if (e.Name.IsEquivalentTo("Data/BigCraftablesInformation")) {
            // See documentation: https://stardewvalleywiki.com/Modding:Items#Big_craftables
            // name / price / edibility / type and category / description / outdoors / indoors / fragility / is lamp / display name
            e.Edit(
                asset => {
                    var data = asset.AsDictionary<int, string>().Data;
                    data.Add(
                        ObjectId,
                        $"{ObjectName}/0/-300/Crafting -9/Item_Challenger_Description/true/true/0//Item_Challenger_Name");
                });
        }
    }
    
    private static bool Draw(
        SObject __instance,
        SpriteBatch spriteBatch, int x, int y, float alpha = 1f) {
        if (IsNotMagicalObject(__instance)) {
            return true;
        }
        GetDelegateObject(__instance).draw(spriteBatch, x, y, alpha);
        return false;
    }

    private static bool IsNotMagicalObject(SObject instance) {
        return instance is not { bigCraftable.Value: true, ParentSheetIndex: ObjectId };
    }

    private static SObject GetDelegateObject(SObject instance) {
        return instance.heldObject.Value ?? DefaultAppearance;
    }
    
    private static bool DrawAsProp(
        SObject __instance,
        SpriteBatch b) {
        if (IsNotMagicalObject(__instance)) {
            return true;
        }
        GetDelegateObject(__instance).drawAsProp(b);
        return false;
    }
    
    private static bool DrawWithAlpha(
        SObject __instance,
        SpriteBatch spriteBatch, int xNonTile, int yNonTile, float layerDepth, float alpha = 1f) {
        if (IsNotMagicalObject(__instance)) {
            return true;
        }
        GetDelegateObject(__instance).draw(spriteBatch, xNonTile, yNonTile, layerDepth, alpha);
        return false;
    }
    
    private static bool DrawInMenu(
        SObject __instance,
        SpriteBatch spriteBatch, Vector2 location, float scaleSize, float transparency, float layerDepth, StackDrawType drawStackNumber, Color color, bool drawShadow) {
        if (IsNotMagicalObject(__instance)) {
            return true;
        }
        GetDelegateObject(__instance).drawInMenu(spriteBatch, location, scaleSize, transparency, layerDepth, drawStackNumber, color, drawShadow);
        return false;
    }
    
    private static bool DrawWhenHeld(
        SObject __instance,
        SpriteBatch spriteBatch, Vector2 objectPosition, Farmer f) {
        if (IsNotMagicalObject(__instance)) {
            return true;
        }
        GetDelegateObject(__instance).drawWhenHeld(spriteBatch, objectPosition, f);
        return false;
    }
}