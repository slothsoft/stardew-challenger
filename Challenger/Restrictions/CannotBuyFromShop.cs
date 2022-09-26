using System.Linq;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Menus;

namespace Slothsoft.Challenger.Restrictions {
    public class CannotBuyFromShop : IRestriction {

        private readonly string[] _bannedShopKeepers;

        public CannotBuyFromShop(params string[] bannedShopKeepers) {
            _bannedShopKeepers = bannedShopKeepers;
        }

        public void Apply(IModHelper modHelper) {
            modHelper.Events.Display.MenuChanged += OnMenuPressed;
        }
        
        private void OnMenuPressed(object sender, MenuChangedEventArgs e) {
            if (e.NewMenu?.GetType() == typeof(ShopMenu)) {
                var newMenu = (ShopMenu) e.NewMenu;
                if (_bannedShopKeepers.Contains(newMenu.portraitPerson.Name)) {
                    newMenu.exitThisMenuNoSound();
                    Game1.addHUDMessage(new HUDMessage("you cannot buy from " + newMenu.portraitPerson.Name, HUDMessage.error_type));
                }
            }
        }

        public void Remove(IModHelper modHelper) {
            modHelper.Events.Display.MenuChanged -= OnMenuPressed;
        }
    }
}