namespace Slothsoft.Challenger.Menus;

internal class ChallengeMenu : BaseOptionsMenu {

    public ChallengeMenu() {
        AddPage(new ChallengePage());
    }
}