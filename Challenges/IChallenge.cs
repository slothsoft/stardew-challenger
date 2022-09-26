using StardewModdingAPI;

namespace Slothsoft.Challenger.Challenges {
    public interface IChallenge {

        void ApplyRestrictions(IModHelper modHelper);
    }
}