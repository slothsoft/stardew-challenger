using System.Linq;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StardewValley.Menus;

namespace Slothsoft.Challenger.Menus;

/// <summary>
/// An implementation of <see cref="IOptionElement"/> that delegates to multiple Stardew Valley
/// <see cref="OptionsElement"/>s.
/// </summary>
internal class MultiOptionElement : IOptionElement {
    
    private readonly int _columnWidth;
    private readonly IOptionElement[] _columns;

    public MultiOptionElement(int columnWidth, params IOptionElement[] optionsElement) {
        _columnWidth = columnWidth;
        _columns = optionsElement;
    }

    public void LeftClickHeld(int x, int y) {
        GetColumn(x)?.LeftClickHeld(x, y);
    }
    
    private IOptionElement? GetColumn(int x) {
        var index = x / _columnWidth;
        if (index < 0 || index >= _columns.Length) {
            return null;
        }
        return _columns[index];
    }

    public void ReceiveLeftClick(int x, int y) {
        GetColumn(x)?.ReceiveLeftClick(x, y);
    }

    public void LeftClickReleased(int x, int y) {
        GetColumn(x)?.LeftClickReleased(x, y);
    }

    public void ReceiveKeyPress(Keys key) {
        foreach (var column in _columns) {
            column.ReceiveKeyPress(key);
        }
    }

    public void Draw(SpriteBatch spriteBatch, int x, int y, IClickableMenu parent) {
        foreach (var column in _columns) {
            column.Draw(spriteBatch, x, y, parent);
        }
    }

    public bool HasActiveOverlay() {
        return _columns.Any(c => c.HasActiveOverlay());
    }
}