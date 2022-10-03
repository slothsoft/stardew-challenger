using Slothsoft.Challenger.Api;
using Slothsoft.Challenger.Models;
using Slothsoft.Challenger.Restrictions;

namespace Slothsoft.Challenger.Challenges;

public class NoCapitalistChallenge : BaseChallenge {
    public NoCapitalistChallenge(IModHelper modHelper) : base(modHelper, "no-capitalist") {
    }

    protected override IRestriction[] CreateRestrictions(IModHelper modHelper) {
        return new IRestriction[] {
            new CannotBuyFromShop(modHelper, Shops.Pierre, Shops.Clint, Shops.JoJo),
        };
    }

    public override MagicalReplacement GetMagicalReplacement() {
        return MagicalReplacement.SeedMaker;
    }
}