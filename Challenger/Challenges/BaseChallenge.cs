using Slothsoft.Challenger.Restrictions;
using StardewModdingAPI;

namespace Slothsoft.Challenger.Challenges {
    public abstract class BaseChallenge : IChallenge {
        
        private IRestriction[] _restrictions;

        public string GetDisplayName(IModHelper modHelper) {
            return modHelper.Translation.Get(GetType().Name);
        }

        public string GetDisplayText(IModHelper modHelper) {
            var result = "";
            foreach (var restriction in GetOrCreateRestrictions()) {
                result += restriction.GetDisplayText(modHelper);
            }
            return result;
        }

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