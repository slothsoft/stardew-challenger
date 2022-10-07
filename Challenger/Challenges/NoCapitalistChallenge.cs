using Slothsoft.Challenger.Api;
using Slothsoft.Challenger.Models;
using Slothsoft.Challenger.Restrictions;

namespace Slothsoft.Challenger.Challenges;

public class NoCapitalistChallenge : BaseChallenge {
    public NoCapitalistChallenge(IModHelper modHelper) : base(modHelper, "no-capitalist") {
    }

    protected override IRestriction[] CreateRestrictions(IModHelper modHelper) {
        return new IRestriction[] {
            new CannotBuyFromShop(modHelper, ShopIds.Pierre, ShopIds.Clint, ShopIds.JoJo),
        };
    }

    public override MagicalReplacement GetMagicalReplacement() {
        return MagicalReplacement.SeedMaker;
    }
}