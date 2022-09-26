using StardewModdingAPI;

namespace Slothsoft.Challenger.Challenges {
    public interface IChallenge {
        /// <summary>
        /// This applies all restriction this challenge has to the game.
        /// </summary>
        /// <param name="modHelper"></param>
        void ApplyRestrictions(IModHelper modHelper);
        /// <summary>
        /// This removes all restriction this challenge has from the game.
        /// </summary>
        /// <param name="modHelper"></param>
        void RemoveRestrictions(IModHelper modHelper);
    }
}