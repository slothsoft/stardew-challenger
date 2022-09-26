using Slothsoft.Challenger.Model;
using Slothsoft.Challenger.Restrictions;

namespace Slothsoft.Challenger.Challenges {
    public class NoCapitalistChallenge : BaseChallenge {

        public NoCapitalistChallenge() : base("no-capitalist") {
        }
        
        protected override IRestriction[] CreateRestrictions() {
            return new IRestriction[]
            {
                new CannotBuyFromShop(Shops.Pierre),
            };
        }
    }
}