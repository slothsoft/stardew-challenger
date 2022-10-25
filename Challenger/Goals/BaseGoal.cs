using System;
using Slothsoft.Challenger.Api;
using Slothsoft.Challenger.Challenges;

namespace Slothsoft.Challenger.Goals;

public abstract class BaseGoal<TProgress> : IGoal
    where TProgress : class {
    private string Id { get; }
    protected IModHelper ModHelper { get; }

    protected TProgress Progress {
        get {
            _progress ??= ReadProgressType();
            return _progress;
        }
    }

    private TProgress? _progress;

    protected BaseGoal(IModHelper modHelper, string id) {
        ModHelper = modHelper;
        Id = id;
    }

    private TProgress ReadProgressType() {
        return ModHelper.Data.ReadSaveData<TProgress>(Id) ?? Activator.CreateInstance<TProgress>();
    }

    protected void WriteProgressType(TProgress progress) {
        _progress = progress;
        ModHelper.Data.WriteSaveData(Id, progress);
    }

    public virtual string GetDisplayName(Difficulty difficulty) {
        return ModHelper.Translation.Get(GetType().Name);
    }

    public abstract void Start();

    public abstract void Stop();

    public abstract bool WasStarted();

    public abstract string GetProgress(Difficulty difficulty);

    public abstract bool IsCompleted(Difficulty difficulty);
}