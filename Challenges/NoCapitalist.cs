using Slothsoft.Challenger.Restrictions;
using StardewModdingAPI;

namespace Slothsoft.Challenger.Challenges {
    public class NoCapitalist : BaseChallenge {
        
        public override void ApplyRestrictions(IModHelper modHelper) {
            new CannotBuyFromShop("Pierre").Apply(modHelper);
        }
    }
}