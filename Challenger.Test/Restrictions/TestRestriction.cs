using System;
using Slothsoft.Challenger.Api;

namespace ChallengerTest.Restrictions; 

public class TestRestriction : IRestriction {

    public string DisplayText { get; set; } = "Test Restriction";
    public Action<TestRestriction> OnApply { get; set; } = _ => {};
    public Action<TestRestriction> OnRemove { get; set; } = _ => {};
    
    public string GetDisplayText() {
        return DisplayText;
    }

    public void Apply() {
        OnApply(this);
    }

    public void Remove() {
        OnRemove(this);
    }
}