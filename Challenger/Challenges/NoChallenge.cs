using System;
using Slothsoft.Challenger.Api;
using Slothsoft.Challenger.Goals;

namespace Slothsoft.Challenger.Challenges;

public class NoChallenge : BaseChallenge {
    public const string ChallengeId = "none";

    public NoChallenge(IModHelper modHelper) : base(modHelper, ChallengeId) {
    }

    public override string GetDisplayText() {
        return ModHelper.Translation.Get("NoChallenge.DisplayText");
    }

    protected override IRestriction[] CreateRestrictions(IModHelper modHelper) {
        return Array.Empty<IRestriction>();
    }
    
    protected override IGoal CreateGoal(IModHelper modHelper) {
        return new NoGoal(modHelper);
    }
}