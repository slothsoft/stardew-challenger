using StardewModdingAPI;

namespace Slothsoft.Challenger.Challenges {
    public interface IChallenge {
        /// <summary>
        /// An ID that never ever changes, so we can be sure to identify challenges.
        /// </summary>
        string Id { get; }
        /// <summary>
        /// Returns the display name of this challenge.
        /// </summary>
        /// <param name="modHelper"></param>
        /// <returns></returns>
        string GetDisplayName(IModHelper modHelper);
        /// <summary>
        /// Returns a string explaining what you can or cannot do in this challenge.
        /// </summary>
        /// <param name="modHelper"></param>
        /// <returns></returns>
        string GetDisplayText(IModHelper modHelper);
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