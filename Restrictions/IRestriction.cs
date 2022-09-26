using StardewModdingAPI;

namespace Slothsoft.Challenger.Restrictions {
    public interface IRestriction {
        
        void Apply(IModHelper modHelper);
    }
}