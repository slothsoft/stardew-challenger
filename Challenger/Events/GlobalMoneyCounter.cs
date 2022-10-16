using System;
using System.Collections.Generic;
using HarmonyLib;
using StardewValley.Menus;

// ReSharper disable InconsistentNaming

namespace Slothsoft.Challenger.Events;

internal static class GlobalMoneyCounter {
    private const string HarmonySuffix = ".GlobalMoneyCounter";

    private static Harmony? _harmony;
    private static readonly IList<Action<Item, int>> SellEvents = new List<Action<Item, int>>();

    private static int? _beforeMoney;
    private static Item? _soldItem;

    public static void AddSellEvent(Action<Item, int> onSellEvent) {
        if (_harmony == null) {
            _harmony = new Harmony(ChallengerMod.Instance.ModManifest.UniqueID + HarmonySuffix);
            _harmony.Patch(
                original: AccessTools.Method(
                    typeof(ShopMenu),
                    nameof(ShopMenu.receiveLeftClick)
                ),
                prefix: new HarmonyMethod(typeof(GlobalMoneyCounter), nameof(MenuReceivingLeftClick)),
                finalizer: new HarmonyMethod(typeof(GlobalMoneyCounter), nameof(MenuReceivedLeftClick))
            );
        }

        SellEvents.Add(onSellEvent);
    }

    public static bool MenuReceivingLeftClick(ShopMenu __instance, int x, int y, bool playSound = true) {
        if (Game1.activeClickableMenu == null)
            return true;
        if (__instance.heldItem == null && !__instance.readOnly) {
            _soldItem = __instance.inventory.getItemAt(x, y);
            _beforeMoney = Game1.player.Money;
        }
        return true;
    }
    
    public static void MenuReceivedLeftClick() {
        if (_soldItem != null) {
            if (_beforeMoney < Game1.player.Money) {
                foreach (var sellEvent in SellEvents) {
                    sellEvent(_soldItem, _beforeMoney == null ? 0 : Game1.player.Money - (int) _beforeMoney);
                }
            }
            _soldItem = null;
            _beforeMoney = null;
        }
    }

    public static void RemoveSellEvent(Action<Item, int> onSellEvent) {
        SellEvents.Remove(onSellEvent);

        if (SellEvents.Count == 0) {
            _harmony?.UnpatchAll(ChallengerMod.Instance.ModManifest.UniqueID + HarmonySuffix);
            _harmony = null;
        }
    }
}