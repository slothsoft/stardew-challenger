using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StardewValley.Menus;

namespace Slothsoft.Challenger.Menus;

/// <summary>
/// An implementation of <see cref="IOptionElement"/> that delegates to the Stardew Valley
/// <see cref="OptionsElement"/>.
/// </summary>
public class SOptionElement : IOptionElement {
    
    private readonly OptionsElement _delegate;

    private bool _held;
    
    public SOptionElement(OptionsElement optionsElement) {
        _delegate = optionsElement;
    }

    public void LeftClickHeld(int x, int y) {
        _delegate.leftClickHeld(x, y);
    }

    public void ReceiveLeftClick(int x, int y) {
        if (_delegate.bounds.Contains(x, y)) {
            _delegate.receiveLeftClick(x, y);
            _held = true;
        }
    }

    public void LeftClickReleased(int x, int y) {
        _delegate.leftClickReleased(x, y);
        _held = false;
    }

    public void ReceiveKeyPress(Keys key) {
        _delegate.receiveKeyPress(key);
    }

    public void Draw(SpriteBatch spriteBatch, int x, int y, IClickableMenu parent) {
        _delegate.draw(spriteBatch, x, y, parent);
    }

    public bool HasActiveOverlay() {
        return _held && _delegate is OptionsDropDown;
    }
}