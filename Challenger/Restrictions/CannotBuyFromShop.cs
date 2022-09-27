using System;
using System.Linq;
using Slothsoft.Challenger.Api;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Menus;

namespace Slothsoft.Challenger.Restrictions {
    public class CannotBuyFromShop : IRestriction {
        
        private readonly string[] _bannedShopKeepers;
        private readonly IModHelper _modHelper;
        
        private EventHandler<MenuChangedEventArgs> _menuChangedHandler;

        public CannotBuyFromShop(IModHelper modHelper, params string[] bannedShopKeepers) {
            _modHelper = modHelper;
            _bannedShopKeepers = bannedShopKeepers;
        }

        public string GetDisplayText() {
            var result = "";
            foreach (var bannedShopKeeper in _bannedShopKeepers) {
                result += "-  " + _modHelper.Translation.Get("CannotBuyFromShop.DisplayText",
                    new { shopKeeper = bannedShopKeeper }) + "\n";
            }
            return result;
        }

        public void Apply() {
            _menuChangedHandler ??= MenuChanged;
            _modHelper.Events.Display.MenuChanged += _menuChangedHandler;
        }

        private void MenuChanged(object sender, MenuChangedEventArgs e) {
            if (e.NewMenu is ShopMenu newMenu) {
                // if the shop has a tool for sale, it's not a shop, but Clint's upgrade function
                if (_bannedShopKeepers.Contains(newMenu.storeContext) && newMenu.forSale.Any(s => s is not Tool)) {
                    newMenu.exitThisMenuNoSound();
                    var shopKeeper = newMenu.portraitPerson?.Name ?? newMenu.storeContext;
                    Game1.addHUDMessage(new HUDMessage(
                        _modHelper.Translation.Get("CannotBuyFromShop.Message", new {  shopKeeper }),
                        HUDMessage.error_type
                    ));
                }
            }
        }

        public void Remove() {
            _modHelper.Events.Display.MenuChanged -= _menuChangedHandler;
        }
    }
}