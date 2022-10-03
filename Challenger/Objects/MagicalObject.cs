using System.Collections.Generic;
using HarmonyLib;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Slothsoft.Challenger.Api;
using Slothsoft.Challenger.Menus;
using Slothsoft.Challenger.Models;
using StardewModdingAPI.Events;

// ReSharper disable InconsistentNaming

namespace Slothsoft.Challenger.Objects;

/// <summary>
/// This class patches <code>StardewValley.Object</code>.
/// </summary>
public static class MagicalObject {

    internal const int ObjectId = 570745037;
    internal const string ObjectName = "Magical Object";
    private const bool ObjectBigCraftable = true;

    private static readonly HashSet<SObject> MagicalObjects = new();

    public static void PatchObject(string uniqueId) {
        var helper = ChallengerMod.Instance.Helper;
        helper.Events.Content.AssetRequested += OnAssetRequested;
        helper.Events.Input.ButtonPressed += OnButtonPressed;

        var harmony = new Harmony(uniqueId);

        MagicalDebris.PatchObject(harmony);
        MagicalGame1.PatchObject(harmony);

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
            prefix: new(typeof(MagicalObject), nameof(MakeVanillaObject)),
            postfix: new(typeof(MagicalObject), nameof(MakeMagicalObject))
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
            prefix: new(typeof(MagicalObject), nameof(MakeVanillaObject)),
            postfix: new(typeof(MagicalObject), nameof(MakeMagicalObject))
        );
        harmony.Patch(
            original: AccessTools.Method(
                typeof(SObject),
                nameof(SObject.drawAsProp),
                new[] {
                    typeof(SpriteBatch),
                }),
            prefix: new(typeof(MagicalObject), nameof(MakeVanillaObject)),
            postfix: new(typeof(MagicalObject), nameof(MakeMagicalObject))
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
            prefix: new(typeof(MagicalObject), nameof(MakeVanillaObject)),
            postfix: new(typeof(MagicalObject), nameof(MakeMagicalObject))
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
            prefix: new(typeof(MagicalObject), nameof(MakeVanillaObject)),
            postfix: new(typeof(MagicalObject), nameof(MakeMagicalObject))
        );

        harmony.Patch(
            original: AccessTools.Method(
                typeof(SObject),
                nameof(SObject.isActionable),
                new[] {
                    typeof(Farmer),
                }),
            prefix: new(typeof(MagicalObject), nameof(MakeVanillaObject)),
            postfix: new(typeof(MagicalObject), nameof(MakeMagicalObject))
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

    private static void OnButtonPressed(object? sender, ButtonPressedEventArgs e) {
        // we don't interact with anything here
        if (!Context.IsPlayerFree) {
            return;
        }

        // Filter out all objects that are not magical or still in use
        var pos = CommonHelpers.GetCursorTile(1);
        if (!Game1.currentLocation.Objects.TryGetValue(pos, out var obj)
            || IsNotMagicalObject(obj)
            || obj.heldObject.Value is not null
            || obj.MinutesUntilReady > 0) {
            return;
        }

        // we have a magical object on our hands
        try {
            MakeVanillaObject(obj);
            if (obj.ParentSheetIndex == MagicalReplacement.Default.ParentSheetIndex) {
                // we have no special item for this challenge -> open challenge Menu
                if (Game1.activeClickableMenu == null && (e.Button.IsActionButton() || e.Button.IsUseToolButton())) 
                    Game1.activeClickableMenu = new ChallengeMenu();
            } else if (e.Button.IsUseToolButton() && Game1.player.CurrentItem != null) {
                var heldItem = (SObject)Game1.player.CurrentItem.getOne();
                if (obj.performObjectDropInAction(heldItem, false, Game1.player)) {
                    Game1.player.reduceActiveItemByOne();
                }
            }
        } finally {
            MakeMagicalObject(obj);
        }
    }

    private static void MakeVanillaObject(SObject __instance) {
        if (IsNotMagicalObject(__instance) || !ChallengerMod.Instance.IsInitialized())
            return;

        MagicalObjects.Add(__instance);

        var magicalReplacement = ChallengerMod.Instance.GetApi()!.GetActiveChallenge().GetMagicalReplacement();
        __instance.ParentSheetIndex = magicalReplacement.ParentSheetIndex;
        __instance.Name = magicalReplacement.Name;
    }

    private static bool IsNotMagicalObject(SObject instance) {
        return instance is not { bigCraftable.Value: ObjectBigCraftable, ParentSheetIndex: ObjectId };
    }

    private static void MakeMagicalObject(SObject __instance) {
        if (!MagicalObjects.Contains(__instance))
            return;

        __instance.ParentSheetIndex = ObjectId;
        __instance.Name = ObjectName;
    }
}