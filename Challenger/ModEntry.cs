using System.Collections.Generic;
using System.Reflection.Emit;
using HarmonyLib;
using Microsoft.Xna.Framework;
using Slothsoft.Challenger.Api;
using Slothsoft.Challenger.Menus;
using Slothsoft.Challenger.Objects;
using StardewModdingAPI.Events;

namespace Slothsoft.Challenger;

public class ModEntry : Mod {
    internal const int ObjectId = 97;
    internal static ModEntry Instance;

    private IChallengerApi _api;

    /// <summary>The mod entry point, called after the mod is first loaded.</summary>
    /// <param name="modHelper">Provides simplified APIs for writing mods.</param>
    public override void Entry(IModHelper modHelper) {
        Instance = this;

        Helper.Events.GameLoop.SaveLoaded += (_, _) => {
            _api = new ChallengerApi(modHelper);
            Monitor.Log($"Challenge \"{_api.GetActiveChallenge().GetDisplayName()}\" was activated.", LogLevel.Debug);
        };
        Helper.Events.Input.ButtonPressed += OnButtonPressed;

        // Events
        Helper.Events.Content.AssetRequested += OnAssetRequested;
        Helper.Events.GameLoop.DayStarted += OnDayStarted;
        Helper.Events.Input.ButtonPressed += OnButtonPressed2;
        Helper.Events.World.ObjectListChanged += OnObjectListChanged;

        // Patches
        var harmony = new Harmony(ModManifest.UniqueID);
        harmony.Patch(
            AccessTools.Method(typeof(SObject), nameof(SObject.checkForAction)),
            transpiler: new(typeof(ModEntry), nameof(Object_checkForAction_transpiler)));
        harmony.Patch(
            AccessTools.Method(typeof(SObject), "getMinutesForCrystalarium"),
            postfix: new(typeof(ModEntry), nameof(Object_getMinutesForCrystalarium_postfix)));
        harmony.Patch(
            AccessTools.Method(typeof(SObject), nameof(SObject.minutesElapsed)),
            new(typeof(ModEntry), nameof(Object_minutesElapsed_prefix)));
    }

    /// <summary>Raised after the player presses a button on the keyboard, controller, or mouse.</summary>
    /// <param name="sender">The event sender.</param>
    /// <param name="e">The event data.</param>
    private void OnButtonPressed(object sender, ButtonPressedEventArgs e) {
        // ignore if player hasn't loaded a save yet
        if (!Context.IsWorldReady)
            return;

        if (e.Button == SButton.J) {
            Game1.player.addItemByMenuIfNecessary(new SObject(Vector2.Zero, ObjectId));
        }

        if (e.Button == SButton.K) {
            Game1.activeClickableMenu = new ChallengeMenu();
        }
    }

    public override IChallengerApi GetApi() => _api;

    public IChallengerApi GetChallengerApi() => _api;

    private static int GetMinutes() {
        return 1;
    }

    private static IEnumerable<CodeInstruction> Object_checkForAction_transpiler(
        IEnumerable<CodeInstruction> instructions) {
        foreach (var instruction in instructions) {
            if (instruction.Calls(AccessTools.Method(typeof(Game1), nameof(Game1.playSound)))) {
                yield return new(OpCodes.Ldarg_0);
                yield return new(OpCodes.Ldloc_1);
                yield return CodeInstruction.Call(typeof(ModEntry), nameof(PlaySound));
                yield return instruction;
            } else {
                yield return instruction;
            }
        }
    }

    private static void Object_getMinutesForCrystalarium_postfix(SObject __instance, ref int __result, int whichGem) {
        if (__instance is not { bigCraftable.Value: true, ParentSheetIndex: ObjectId }) {
            return;
        }

        var minutes = GetMinutes();
        if (minutes == 0) {
            return;
        }

        __result = minutes;
    }

    private static bool Object_minutesElapsed_prefix(SObject __instance) {
        if (__instance is not { bigCraftable.Value: true, Name: "Crystalarium", ParentSheetIndex: ObjectId }) {
            return true;
        }

        return __instance.heldObject.Value is not null;
    }

    private static void OnObjectListChanged(object? sender, ObjectListChangedEventArgs e) {
        foreach (var (_, obj) in e.Added) {
            if (obj is not { bigCraftable.Value: true, ParentSheetIndex: ObjectId }) {
                continue;
            }

            obj.Name = "Crystalarium";
        }
    }

    private static string PlaySound(string sound, SObject obj, SObject? heldObj) {
        return sound;
    }

    private void OnAssetRequested(object? sender, AssetRequestedEventArgs e) {
        if (e.Name.IsEquivalentTo("Data/CraftingRecipes")) {
            e.Edit(
                asset => {
                    var data = asset.AsDictionary<string, string>().Data;
                    data.Add(
                        "Ordinary Capsule",
                        $"335 1/Home/{ObjectId}/true/null/Item_Challenger_Name");
                });
            return;
        }

        if (e.Name.IsEquivalentTo("Data/BigCraftablesInformation")) {
            // See documentation: https://stardewvalleywiki.com/Modding:Items#Big_craftables
            // name / price / edibility / type and category / description / outdoors / indoors / fragility / is lamp / display name
            e.Edit(
                asset => {
                    var data = asset.AsDictionary<int, string>().Data;
                    data.Add(
                        ObjectId,
                        $"Ordinary Capsule/0/-300/Crafting -9/Item_Challenger_Description/true/true/0//Item_Challenger_Name");
                });
        }
    }

    private void OnButtonPressed2(object? sender, ButtonPressedEventArgs e) {
        if (!Context.IsPlayerFree
            || Game1.player.CurrentItem is not SObject { bigCraftable.Value: false }
            || !e.Button.IsUseToolButton()) {
            return;
        }

        var pos = CommonHelpers.GetCursorTile(1);
        if (!Game1.currentLocation.Objects.TryGetValue(pos, out var obj)
            || obj is not { bigCraftable.Value: true, Name: "Crystalarium", ParentSheetIndex: ObjectId }
            || obj.heldObject.Value is not null
            || obj.MinutesUntilReady > 0) {
            return;
        }

        obj.heldObject.Value = (SObject)Game1.player.CurrentItem.getOne();
        Game1.currentLocation.playSound("select");
        obj.MinutesUntilReady = 1;
        Game1.player.reduceActiveItemByOne();
        Helper.Input.Suppress(e.Button);
    }

    private void OnDayStarted(object? sender, DayStartedEventArgs e) {
        if (!Game1.player.craftingRecipes.ContainsKey("Ordinary Capsule")) {
            Game1.player.craftingRecipes.Add("Ordinary Capsule", 0);
        }
    }
}