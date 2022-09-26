using Slothsoft.Challenger.Restrictions;

namespace Slothsoft.Challenger.Challenges {
    public class NoCapitalist : BaseChallenge {
        
        protected override IRestriction[] CreateRestrictions() {
            return new IRestriction[]
            {
                new CannotBuyFromShop("Pierre"),
            };
        }
    }
}