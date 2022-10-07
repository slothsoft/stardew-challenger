﻿using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using StardewValley.Menus;

// ReSharper disable InconsistentNaming

namespace Slothsoft.Challenger.Restrictions;

internal static class GlobalCarpenterChanger {
    
    private static Harmony? _harmony;
    private static readonly IList<string> ExcludedBluePrintNames = new List<string>();

    public static void AddExcludedBluePrintNames(IEnumerable<string> excludedBluePrintNames) {
        if (_harmony == null) {
            _harmony = new Harmony(ChallengerMod.Instance.ModManifest.UniqueID + ".GlobalCarpenterChanger");
            _harmony.Patch(
                original: AccessTools.Constructor(
                    typeof(CarpenterMenu),
            new[] {
                        typeof(bool),
                    }
                ),
                postfix: new(typeof(GlobalCarpenterChanger), nameof(ChangeBluePrints))
            );
        }
        foreach (var excludedBluePrintName in excludedBluePrintNames) {
            ExcludedBluePrintNames.Add(excludedBluePrintName);
        }
    }

    public static void ChangeBluePrints(CarpenterMenu __instance) {
        var blueprintsField = Traverse.Create(__instance).Field<List<BluePrint>>("blueprints");
        var blueprints = blueprintsField.Value;
        var newBlueprints = blueprints.Where(b => !ExcludedBluePrintNames.Contains(b.name)).ToList();
        blueprintsField.Value = newBlueprints;
    }

    public static void RemoveExcludedBluePrintNames(IEnumerable<string> excludedBluePrintNames) {
        foreach (var excludedBluePrintName in excludedBluePrintNames) {
            ExcludedBluePrintNames.Remove(excludedBluePrintName);
        }

        if (ExcludedBluePrintNames.Count == 0) {
            _harmony?.UnpatchAll();
            _harmony = null;
        }
    }
}