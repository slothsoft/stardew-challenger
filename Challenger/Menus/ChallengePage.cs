using System.Linq;
using Microsoft.Xna.Framework;
using StardewValley.Menus;

namespace Slothsoft.Challenger.Menus;

public class ChallengePage : OptionsPage {
    private readonly OptionsDropDown _challengeSelection;
    private OptionsElement _goal;
    private OptionsElement _description;

    public ChallengePage(int x, int y, int width, int height) : base(x, y, width, height) {
        options.Clear();
        options.Add(new OptionsElement(ChallengerMod.Instance.Helper.Translation.Get("ChallengePage.Title") + ":"));

        _challengeSelection = new OptionsDropDown("", -1);
        _challengeSelection.bounds = new Rectangle(
            _challengeSelection.bounds.X,
            _challengeSelection.bounds.Y,
            width - 3 * _challengeSelection.bounds.X,
            _challengeSelection.bounds.Height);
        options.Add(_challengeSelection);

        _goal = CreateOptionsElement("\n");
        options.Add(_goal);
        
        _description = CreateOptionsElement();
        options.Add(_description);

        var api = ChallengerMod.Instance.GetApi()!;
        var activeChallenge = api.GetActiveChallenge();
        var allChallenges = api.GetAllChallenges().ToArray();

        var selectedOption = 0;

        for (var i = 0; i < allChallenges.Length; i++) {
            var challenge = allChallenges[i];
            if (activeChallenge.Id == challenge.Id) {
                selectedOption = i;
            }

            _challengeSelection.dropDownOptions.Add(challenge.Id);
            _challengeSelection.dropDownDisplayOptions.Add(challenge.GetDisplayName());
        }

        _challengeSelection.selectedOption = selectedOption;
        _challengeSelection.RecalculateBounds();
        RefreshDescriptionLabel(false);
    }

    private static OptionsElement CreateOptionsElement(string label = "") {
        var result = new OptionsElement(label) {
            style = OptionsElement.Style.OptionLabel
        };
        var descriptionSize = Game1.smallFont.MeasureString(result.label);
        result.bounds = new Rectangle(result.bounds.X, result.bounds.Y, (int)descriptionSize.X,
            (int)descriptionSize.Y);
        return result;
    }

    public override void releaseLeftClick(int x, int y) {
        base.releaseLeftClick(x, y);
        RefreshDescriptionLabel(true);
    }

    private void RefreshDescriptionLabel(bool saveAllowed) {
        var api = ChallengerMod.Instance.GetApi()!;
        var allChallenges = api.GetAllChallenges().ToArray();
        var newChallenge = allChallenges[_challengeSelection.selectedOption];
        var newLabel = newChallenge.GetDisplayText();

        if (_description.label != newLabel) {
            _description.label = newLabel;

            var challengeGoal = newChallenge.GetGoal();
            var goal = ChallengerMod.Instance.Helper.Translation.Get("ChallengePage.Goal");
            _goal.label = $"{goal}: {challengeGoal.GetDisplayName()}aw";

            if (challengeGoal.WasStarted()) {
                _goal.label += $"\n      ({challengeGoal.GetProgress()})";
            }

            if (saveAllowed) {
                api.SetActiveChallenge(newChallenge);
            }
        }
    }
}