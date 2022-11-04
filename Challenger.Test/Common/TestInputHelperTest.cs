﻿using NUnit.Framework;
using StardewModdingAPI.Utilities;

namespace ChallengerTest.Common; 

public class TestInputHelperTest {
    
    private TestInputHelper _classUnderTest = new();

    [SetUp]
    public void SetUp() {
        _classUnderTest = new();
    }
    
    [Test]
    public void Suppress() {
        _classUnderTest.Suppress(SButton.B);
        
        Assert.IsFalse(_classUnderTest.IsSuppressed(SButton.A));
        Assert.IsTrue(_classUnderTest.IsSuppressed(SButton.B));
    }

    [Test]
    public void SuppressActiveKeybinds() {
        var keybinds = new KeybindList(new Keybind(SButton.A), new Keybind(SButton.C));
        _classUnderTest.SuppressActiveKeybinds(keybinds);
        
        Assert.IsTrue(_classUnderTest.IsSuppressed(SButton.A));
        Assert.IsFalse(_classUnderTest.IsSuppressed(SButton.B));
        Assert.IsTrue(_classUnderTest.IsSuppressed(SButton.C));
    }
    
    [Test]
    public void GetCursorPosition() {
        Assert.NotNull(_classUnderTest.GetCursorPosition());
    }

    [Test]
    public void Down() {
        _classUnderTest.Down(SButton.B);
        
        Assert.IsFalse(_classUnderTest.IsDown(SButton.A));
        Assert.IsTrue(_classUnderTest.IsDown(SButton.B));
    }

    [Test]
    public void GetState() {
        _classUnderTest.Suppress(SButton.A);
        _classUnderTest.Down(SButton.B);
        
        Assert.AreEqual(SButtonState.None, _classUnderTest.GetState(SButton.A));
        Assert.AreEqual(SButtonState.Held, _classUnderTest.GetState(SButton.B));
        Assert.AreEqual(SButtonState.Released, _classUnderTest.GetState(SButton.C));
    }
}