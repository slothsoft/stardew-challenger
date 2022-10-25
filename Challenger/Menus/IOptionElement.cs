using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StardewValley.Menus;

namespace Slothsoft.Challenger.Menus; 

/// <summary>
/// A general option element that can be clicked and scrolled on an <see cref="BaseOptionsPage"/>.
/// </summary>

internal interface IOptionElement {
    void LeftClickHeld(int x, int y);
    void ReceiveLeftClick(int x, int y);
    void LeftClickReleased(int x, int y);
    void ReceiveKeyPress(Keys key);
    void Draw(SpriteBatch spriteBatch, int x, int y, IClickableMenu parent);
    bool HasActiveOverlay();
}