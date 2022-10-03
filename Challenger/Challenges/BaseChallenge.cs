using Slothsoft.Challenger.Api;
using Slothsoft.Challenger.Models;

namespace Slothsoft.Challenger.Challenges;

public abstract class BaseChallenge : IChallenge {
    public string Id { get; }
    protected IModHelper ModHelper { get; }

    private IRestriction[] _restrictions;

    protected BaseChallenge(IModHelper modHelper, string id) {
        ModHelper = modHelper;
        Id = id;
    }

    public string GetDisplayName() {
        return ModHelper.Translation.Get(GetType().Name);
    }

    public virtual string GetDisplayText() {
        var result = "";
        foreach (var restriction in GetOrCreateRestrictions()) {
            result += restriction.GetDisplayText();
        }
        var magicalReplacement = GetMagicalReplacement();
        if (magicalReplacement != MagicalReplacement.Default) {
            result += CommonHelpers.ToListString(ModHelper.Translation.Get("BaseChallenge.MagicalObject",
                new { item = magicalReplacement.Name }).ToString());
        }
        return result;
    }

    public void ApplyRestrictions() {
        foreach (var restriction in GetOrCreateRestrictions()) {
            restriction.Apply();
        }
    }

    private IRestriction[] GetOrCreateRestrictions() {
        _restrictions ??= CreateRestrictions(ModHelper);
        return _restrictions;
    }

    protected abstract IRestriction[] CreateRestrictions(IModHelper modHelper);

    public void RemoveRestrictions() {
        foreach (var restriction in GetOrCreateRestrictions()) {
            restriction.Remove();
        }
    }

    public virtual MagicalReplacement GetMagicalReplacement() {
        return MagicalReplacement.Default;
    }
}