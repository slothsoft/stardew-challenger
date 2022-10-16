﻿using Slothsoft.Challenger.Api;

namespace Slothsoft.Challenger.Goals; 

public class PerfectionGoal : IGoal {

    private readonly IModHelper _modHelper;
    
    public PerfectionGoal(IModHelper modHelper) {
        _modHelper = modHelper;
    }

    public string GetDisplayName() {
        return _modHelper.Translation.Get(GetType().Name);
    }
    
    public bool IsCompleted() {
        return Utility.percentGameComplete() >= 1;
    }

    public void Start() {
        // we don't need to start this
    }

    public void Stop() {
        // we don't need to stop this
    }

    public bool WasStarted() {
        return Utility.percentGameComplete() > 0;
    }

    public string GetProgress() {
        return $"{(Utility.percentGameComplete() * 100):0} / 100%";
    }
}