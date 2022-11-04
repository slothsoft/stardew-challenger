using System;
using StardewModdingAPI.Events;

namespace ChallengerTest.Common.Events; 

public class TestSpecializedEvents : ISpecializedEvents{
    public event EventHandler<LoadStageChangedEventArgs>? LoadStageChanged;
    public event EventHandler<UnvalidatedUpdateTickingEventArgs>? UnvalidatedUpdateTicking;
    public event EventHandler<UnvalidatedUpdateTickedEventArgs>? UnvalidatedUpdateTicked;
}