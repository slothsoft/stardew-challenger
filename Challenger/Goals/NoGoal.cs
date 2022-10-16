using Slothsoft.Challenger.Api;

namespace Slothsoft.Challenger.Goals; 

public class NoGoal : IGoal {

    private readonly IModHelper _modHelper;
    
    public NoGoal(IModHelper modHelper) {
        _modHelper = modHelper;
    }

    public string GetDisplayName() {
        return _modHelper.Translation.Get(GetType().Name);
    }
    
    public bool IsCompleted() {
        return false;
    }

    public void Start() {
        // we don't need to start this
    }

    public void Stop() {
        // we don't need to stop this
    }

    public bool WasStarted() {
        return false;
    }

    public string GetProgress() {
        return "";
    }
}