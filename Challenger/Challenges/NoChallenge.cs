using System;
using Slothsoft.Challenger.Restrictions;
using StardewModdingAPI;

namespace Slothsoft.Challenger.Challenges {
    public class NoChallenge : BaseChallenge {
        
        public NoChallenge() : base("none") {
        }
        
        public override string GetDisplayText(IModHelper modHelper) {
            return modHelper.Translation.Get("NoChallenge.DisplayText");
        }

        protected override IRestriction[] CreateRestrictions() {
            return Array.Empty<IRestriction>();
        }
    }
}