using System;
using Slothsoft.Challenger.Api;

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
}