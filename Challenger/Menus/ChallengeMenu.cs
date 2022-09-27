using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StardewValley;
using StardewValley.Menus;

namespace Slothsoft.Challenger.Menus {
    internal class ChallengeMenu : IClickableMenu {
        
        private readonly IClickableMenu _page;

        public ChallengeMenu()
            : base(Game1.viewport.Width / 2 - (800 + borderWidth * 2) / 2,
                Game1.viewport.Height / 2 - (600 + borderWidth * 2) / 2,
                800 + borderWidth * 2, 600 + borderWidth * 2, true) {
            _page = new ChallengePage(xPositionOnScreen, yPositionOnScreen,
                width + (LocalizedContentManager.CurrentLanguageCode == LocalizedContentManager.LanguageCode.ru
                    ? Game1.tileSize * 3 / 2
                    : Game1.tileSize / 2), height);
            if (Game1.activeClickableMenu == null)
                Game1.playSound("bigSelect");
            if (!Game1.options.SnappyMenus)
                return;
            _page.populateClickableComponentList();
        }

        public override void setUpForGamePadMode() {
            base.setUpForGamePadMode();
            _page.setUpForGamePadMode();
        }

        public override ClickableComponent getCurrentlySnappedComponent() {
            return _page.getCurrentlySnappedComponent();
        }

        public override void setCurrentlySnappedComponentTo(int id) {
            _page.setCurrentlySnappedComponentTo(id);
        }

        public override void receiveLeftClick(int x, int y, bool playSound = true) {
            base.receiveLeftClick(x, y, playSound);
            _page.receiveLeftClick(x, y);
        }

        public override void receiveRightClick(int x, int y, bool playSound = true) {
            _page.receiveRightClick(x, y, playSound);
        }

        public override void receiveScrollWheelAction(int direction) {
            base.receiveScrollWheelAction(direction);
            _page.receiveScrollWheelAction(direction);
        }

        public override void performHoverAction(int x, int y) {
            base.performHoverAction(x, y);
            _page.performHoverAction(x, y);
        }

        public override void releaseLeftClick(int x, int y) {
            base.releaseLeftClick(x, y);
            _page.releaseLeftClick(x, y);
        }

        public override void leftClickHeld(int x, int y) {
            base.leftClickHeld(x, y);
            _page.leftClickHeld(x, y);
        }

        public override bool readyToClose() {
            return _page.readyToClose();
        }

        public override void draw(SpriteBatch b) {
            if (!Game1.options.showMenuBackground)
                b.Draw(Game1.fadeToBlackRect, Game1.graphics.GraphicsDevice.Viewport.Bounds, Color.Black * 0.4f);
            Game1.drawDialogueBox(xPositionOnScreen, yPositionOnScreen, _page.width, _page.height, false,
                true);
            _page.draw(b);
            b.End();
            b.Begin(SpriteSortMode.FrontToBack, BlendState.NonPremultiplied, SamplerState.PointClamp);

            base.draw(b);
            if (Game1.options.hardwareCursor)
                return;
            b.Draw(Game1.mouseCursors, new Vector2(Game1.getOldMouseX(), Game1.getOldMouseY()),
                Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors,
                    Game1.options.gamepadControls ? 44 : 0, 16, 16), Color.White, 0.0f, Vector2.Zero,
                Game1.pixelZoom + Game1.dialogueButtonScale / 150f, SpriteEffects.None, 1f);
        }

        public override bool areGamePadControlsImplemented() {
            return false;
        }

        public override void receiveKeyPress(Keys key) {
            if (Game1.options.menuButton.Contains(new InputButton(key)) &&
                readyToClose()) {
                Game1.exitActiveMenu();
                Game1.playSound("bigDeSelect");
            }
            _page.receiveKeyPress(key);
        }
    }
}