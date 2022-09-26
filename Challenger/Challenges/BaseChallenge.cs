using Slothsoft.Challenger.Restrictions;
using StardewModdingAPI;

namespace Slothsoft.Challenger.Challenges {
    public abstract class BaseChallenge : IChallenge {
        
        public string Id { get; }
        
        private IRestriction[] _restrictions;

        protected BaseChallenge(string id) {
            Id = id;
        }

        public string GetDisplayName(IModHelper modHelper) {
            return modHelper.Translation.Get(GetType().Name);
        }

        public virtual string GetDisplayText(IModHelper modHelper) {
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