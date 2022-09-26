using Slothsoft.Challenger.Restrictions;
using StardewModdingAPI;

namespace Slothsoft.Challenger.Challenges {
    public abstract class BaseChallenge : IChallenge {
        
        private IRestriction[] _restrictions;

        public void ApplyRestrictions(IModHelper modHelper) {
            foreach (var restriction in GetOrCreateRestrictions()) {
                restriction.Apply(modHelper);
            }
        }

        private IRestriction[] GetOrCreateRestrictions() {
            _restrictions ??= CreateRestrictions();
            return _restrictions;
        }

        protected abstract IRestriction[] CreateRestrictions();

        public void RemoveRestrictions(IModHelper modHelper) {
            foreach (var restriction in GetOrCreateRestrictions()) {
                restriction.Remove(modHelper);
            }
        }
    }
}