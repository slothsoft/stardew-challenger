using StardewModdingAPI;

namespace Slothsoft.Challenger.Restrictions {
    public interface IRestriction {
        
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