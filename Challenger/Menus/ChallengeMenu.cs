using Microsoft.Xna.Framework;

namespace Slothsoft.Challenger.Menus;

internal class ChallengeMenu : BaseOptionsMenu {

    public ChallengeMenu() {
        var pageBounds = CreatePageBounds();
        AddPage(new ChallengePage(pageBounds));
    }

    private Rectangle CreatePageBounds() {
        return new Rectangle(
            xPositionOnScreen, 
            yPositionOnScreen,
            width + (LocalizedContentManager.CurrentLanguageCode == LocalizedContentManager.LanguageCode.ru ? Game1.tileSize * 3 / 2 : Game1.tileSize / 2), 
            height
        );
    }
}