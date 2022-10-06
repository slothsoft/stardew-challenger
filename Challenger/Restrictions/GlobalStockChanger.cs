using System;
using System.Collections.Generic;
using HarmonyLib;
using StardewValley.Locations;
// ReSharper disable InconsistentNaming

namespace Slothsoft.Challenger.Restrictions;

internal static class GlobalStockChanger {
    
    private static Harmony? _harmony;
    private static readonly IList<Action<IDictionary<ISalable, int[]>>> ChangerList =
        new List<Action<IDictionary<ISalable, int[]>>>();

    public static void AddChanger(Action<IDictionary<ISalable, int[]>> changer) {
        if (_harmony == null) {
            _harmony = new Harmony(ChallengerMod.Instance.ModManifest.UniqueID + ".GlobalStockChanger");
            _harmony.Patch(
                original: AccessTools.Method(
                    typeof(SeedShop),
                    nameof(SeedShop.shopStock)
                ),
                postfix: new(typeof(GlobalStockChanger), nameof(ChangeShopStock))
            );
            _harmony.Patch(
                original: AccessTools.Method(
                    typeof(Utility),
                    nameof(Utility.getJojaStock)
                ),
                postfix: new(typeof(GlobalStockChanger), nameof(ChangeShopStock))
            );
        }
        ChangerList.Add(changer);
    }

    public static void ChangeShopStock(ref Dictionary<ISalable, int[]> __result) {
        foreach (var changer in ChangerList) {
            changer(__result);
        }
    }

    public static void RemoveChanger(Action<IDictionary<ISalable, int[]>> changer) {
        ChangerList.Remove(changer);

        if (ChangerList.Count == 0) {
            _harmony?.UnpatchAll();
            _harmony = null;
        }
    }
}