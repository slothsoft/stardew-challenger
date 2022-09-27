using StardewModdingAPI;

namespace Slothsoft.Challenger.Api {
    public interface IRestriction {
        /// <summary>
        /// Returns a string explaining what you can or cannot do in this restriction.
        /// </summary>
        /// <returns></returns>
        string GetDisplayText();
        
        /// <summary>
        /// Adds this restriction to the game.
        /// </summary>
        void Apply();

        /// <summary>
        /// Removes this restriction from the game.
        /// </summary>
        void Remove();
    }
}