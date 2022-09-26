using StardewModdingAPI;

namespace Slothsoft.Challenger.Challenges {
    public abstract class BaseChallenge : IChallenge {
        
        public abstract void ApplyRestrictions(IModHelper modHelper);
    }
}