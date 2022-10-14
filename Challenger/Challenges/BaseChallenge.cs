using Slothsoft.Challenger.Api;
using Slothsoft.Challenger.Goals;
using Slothsoft.Challenger.Models;

namespace Slothsoft.Challenger.Challenges;

public abstract class BaseChallenge : IChallenge {
    public string Id { get; }
    protected IModHelper ModHelper { get; }

    private IRestriction[]? _restrictions;
    private string? _magicalReplacementName;
    private IGoal? _goal;

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

        var magicalReplacementName = FetchMagicalReplacementName();
        if (magicalReplacementName.Length > 0) { // if empty it's the default or completely broken
            result += CommonHelpers.ToListString(ModHelper.Translation.Get("BaseChallenge.MagicalObject",
                new { item = magicalReplacementName }).ToString());
        }
        return result;
    }

    private string FetchMagicalReplacementName() {
        if (_magicalReplacementName == null) {
            var magicalReplacement = GetMagicalReplacement();
            if (magicalReplacement != MagicalReplacement.Default) {
                Game1.bigCraftablesInformation.TryGetValue(magicalReplacement.ParentSheetIndex, out var info);
                if (info != null) {
                    var split = info.Split('/');
                    if (split.Length > 8) {
                        _magicalReplacementName = split[8];
                    } else {
                        ChallengerMod.Instance.Monitor.Log($"BaseChallenge found info string of {magicalReplacement.ParentSheetIndex} with missing name: {info}", LogLevel.Error);
                    }
                } else {
                    ChallengerMod.Instance.Monitor.Log($"BaseChallenge could not find info string of {magicalReplacement.ParentSheetIndex}", LogLevel.Error);
                }
            }
            _magicalReplacementName ??= "";
        }
        return _magicalReplacementName;
    }

    public void Start() {
        foreach (var restriction in GetOrCreateRestrictions()) {
            restriction.Apply();
        }
        GetGoal().Start();
    }

    private IRestriction[] GetOrCreateRestrictions() {
        _restrictions ??= CreateRestrictions(ModHelper);
        return _restrictions;
    }

    protected abstract IRestriction[] CreateRestrictions(IModHelper modHelper);

    public void Stop() {
        foreach (var restriction in GetOrCreateRestrictions()) {
            restriction.Remove();
        }
        GetGoal().Stop();
    }

    public virtual MagicalReplacement GetMagicalReplacement() {
        return MagicalReplacement.Default;
    }

    public IGoal GetGoal() {
        _goal ??= CreateGoal(ModHelper);
        return _goal;
    }

    protected virtual IGoal CreateGoal(IModHelper modHelper) {
        return new PerfectionGoal(ModHelper);
    }

    public bool IsCompleted() {
        return GetGoal().IsCompleted();
    }
}