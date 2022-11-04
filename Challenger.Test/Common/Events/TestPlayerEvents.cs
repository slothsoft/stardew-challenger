﻿using System;
using StardewModdingAPI.Events;

namespace ChallengerTest.Common.Events; 

public class TestPlayerEvents : IPlayerEvents {
    public event EventHandler<InventoryChangedEventArgs>? InventoryChanged;
    public event EventHandler<LevelChangedEventArgs>? LevelChanged;
    public event EventHandler<WarpedEventArgs>? Warped;
}