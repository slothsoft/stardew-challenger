using System;
using Slothsoft.Challenger.Api;
using Slothsoft.Challenger.Challenges;

namespace ChallengerTest.Goals;

public class TestBaseGoal : IGoal {
    
    public string DisplayName { get; set; } = "Test Base Goal";    
    public bool Started { get; set; }
    public string Progress { get; set; } = ""; 
    public bool Completed { get; set; }
    public Action<TestBaseGoal> OnStart { get; set; } = _ => {};
    public Action<TestBaseGoal> OnStop { get; set; } = _ => {};

    public string GetDisplayName(Difficulty difficulty) {
        return DisplayName;
    }

    public bool WasStarted() {
        return Started;
    }

    public string GetProgress(Difficulty difficulty) {
        return Progress;
    }

    public bool IsCompleted(Difficulty difficulty) {
        return Completed;
    }

    public void Start() {
        OnStart(this);
    }

    public void Stop() {
        OnStop(this);
    }
}