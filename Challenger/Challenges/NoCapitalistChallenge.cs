using Slothsoft.Challenger.Api;
using Slothsoft.Challenger.Goals;
using Slothsoft.Challenger.Models;
using Slothsoft.Challenger.Restrictions;

namespace Slothsoft.Challenger.Challenges;

public class NoCapitalistChallenge : BaseChallenge {
    public NoCapitalistChallenge(IModHelper modHelper) : base(modHelper, "no-capitalist") {
    }

    protected override IRestriction[] CreateRestrictions(IModHelper modHelper, Difficulty difficulty) {
        if (difficulty == Difficulty.Hard) {
            return new IRestriction[] {
                new CannotBuyFromShop(modHelper, ShopIds.Pierre, ShopIds.Clint, ShopIds.JoJo),
            };
        }
        return new IRestriction[] {
            new CannotBuyFromShop(modHelper, ShopIds.Pierre, ShopIds.JoJo),
        };
    }

    public override MagicalReplacement GetMagicalReplacement(Difficulty difficulty) {
        return difficulty == Difficulty.Easy ? MagicalReplacement.SeedMaker : MagicalReplacement.Default;
    }

    protected override IGoal CreateGoal(IModHelper modHelper) {
        return new CommunityCenterGoal(modHelper);
    }
}