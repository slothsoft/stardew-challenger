﻿using System;
using StardewModdingAPI.Events;

namespace ChallengerTest.Common.Events; 

public class TestInputEvents : IInputEvents{
    public event EventHandler<ButtonsChangedEventArgs>? ButtonsChanged;
    public event EventHandler<ButtonPressedEventArgs>? ButtonPressed;
    public event EventHandler<ButtonReleasedEventArgs>? ButtonReleased;
    public event EventHandler<CursorMovedEventArgs>? CursorMoved;
    public event EventHandler<MouseWheelScrolledEventArgs>? MouseWheelScrolled;
}