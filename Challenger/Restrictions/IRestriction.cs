using StardewModdingAPI;

namespace Slothsoft.Challenger.Restrictions {
    public interface IRestriction {
        /// <summary>
        /// Returns a string explaining what you can or cannot do in this restriction.
        /// </summary>
        /// <param name="modHelper"></param>
        /// <returns></returns>
        string GetDisplayText(IModHelper modHelper);
        
        /// <summary>
        /// Adds this restriction to the game.
        /// </summary>
        /// <param name="modHelper"></param>
        void Apply(IModHelper modHelper);

        /// <summary>
        /// Removes this restriction from the game.
        /// </summary>
        /// <param name="modHelper"></param>
        void Remove(IModHelper modHelper);
    }
}