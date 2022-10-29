using System;
using Slothsoft.Challenger.Api;
using Slothsoft.Challenger.Goals;

namespace Slothsoft.Challenger.Challenges;

public class EarnMoneyChallenge : BaseChallenge {
    public EarnMoneyChallenge(IModHelper modHelper) : base(modHelper, "earn-money") {
    }

    protected override IRestriction[] CreateRestrictions(IModHelper modHelper, Difficulty difficulty) {
        return Array.Empty<IRestriction>();
    }

    protected override IGoal CreateGoal(IModHelper modHelper) {
        return new EarnMoneyGoal(modHelper, CalculateTargetMoney);
    }

    internal static int CalculateTargetMoney(Difficulty difficulty) {
        switch (difficulty) {
            case Difficulty.Easy:
                return 2_500_000;
            case Difficulty.Medium:
                return 5_000_000;
            default:
                return 10_000_000;
        }
    }
}