using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Slothsoft.Challenger.Api;
using Slothsoft.Challenger.Challenges;
using StardewValley.Menus;

namespace Slothsoft.Challenger.Menus;

internal class ChallengePage : BaseOptionsPage {
    
    private readonly OptionsDropDown _challengeSelection;
    private readonly OptionsDropDown _difficultySelection;
    private readonly OptionsElement _goal;
    private readonly OptionsElement _description;

    private string? _lastUpdatedChallenge;
    
    public ChallengePage() {
        
        _difficultySelection = new OptionsDropDown("", -1);
        AddOption(new MultiOptionElement(
            InitSingleLineElement(new OptionsElement(ChallengerMod.Instance.Helper.Translation.Get("ChallengePage.Title") + ":")),
            InitSingleLineElement(_difficultySelection)));

        _challengeSelection = new OptionsDropDown("", -1);
        AddOption(_challengeSelection);
        
        _goal = CreateOptionsElement("\n");
        AddOption(_goal);
        
        _description = CreateOptionsElement();
        AddOption(_description);
        
        AddOption(new OptionsDropDown("", -1));
        
        var api = ChallengerMod.Instance.GetApi()!;
        var activeChallenge = api.ActiveChallenge;
        var activeDifficulty = api.ActiveDifficulty;
        var allChallenges = api.GetAllChallenges().ToArray();

        var selectedOption = 0;

        for (var i = 0; i < allChallenges.Length; i++) {
            var challenge = allChallenges[i];
            if (activeChallenge.Id == challenge.Id) {
                selectedOption = i;
            }

            _challengeSelection.dropDownOptions.Add(challenge.Id);
            _challengeSelection.dropDownDisplayOptions.Add(challenge.DisplayName);
        }

        _challengeSelection.selectedOption = selectedOption;
        _challengeSelection.RecalculateBounds();
        
        foreach (var difficulty in (Difficulty[]) Enum.GetValues(typeof(Difficulty))) {
            _difficultySelection.dropDownOptions.Add(difficulty.ToString());
            _difficultySelection.dropDownDisplayOptions.Add(difficulty.ToString());
        }
        _difficultySelection.selectedOption = (int) activeDifficulty;
        _difficultySelection.RecalculateBounds();
        
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
        var newDifficulty = (Difficulty) _difficultySelection.selectedOption;

        var updatedChallenge = newChallenge.Id + "-" + newDifficulty;

        if (updatedChallenge != _lastUpdatedChallenge) {
            if (newChallenge.Id == NoChallenge.ChallengeId) {
                // no challenge will only display its description as the goal and nothing else
                _goal.label = newChallenge.GetDisplayText(newDifficulty); 
                _description.label = "";
            } else {
                _description.label = newChallenge.GetDisplayText(newDifficulty); 
                
                var goal = ChallengerMod.Instance.Helper.Translation.Get("ChallengePage.Goal");
                _goal.label = $"{goal}: {newChallenge.GetGoalDisplayName(newDifficulty)}";

                if (newChallenge.WasStarted()) {
                    _goal.label += $"\n      ({newChallenge.GetProgress(newDifficulty)})";
                }
            }

            if (saveAllowed) {
                api.ActiveChallenge = newChallenge;
                api.ActiveDifficulty = newDifficulty;
            }
            _lastUpdatedChallenge = updatedChallenge;
        }
    }
}