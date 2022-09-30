using Microsoft.Xna.Framework;
using Slothsoft.Challenger.Api;
using Slothsoft.Challenger.Menus;
using Slothsoft.Challenger.Objects;
using StardewModdingAPI.Events;

namespace Slothsoft.Challenger;

public class ChallengerMod : Mod {
    internal static ChallengerMod Instance;

    private IChallengerApi _api;

    /// <summary>The mod entry point, called after the mod is first loaded.</summary>
    /// <param name="modHelper">Provides simplified APIs for writing mods.</param>
    public override void Entry(IModHelper modHelper) {
        Instance = this;

        Helper.Events.GameLoop.SaveLoaded += (_, _) => {
            _api = new ChallengerApi(modHelper);
            Monitor.Log($"Challenge \"{_api.GetActiveChallenge().GetDisplayName()}\" was activated.", LogLevel.Debug);
        };
        Helper.Events.Input.ButtonPressed += OnButtonPressed;

        // Patches
        MagicalObject.PatchObject(ModManifest.UniqueID);
    }

    /// <summary>Raised after the player presses a button on the keyboard, controller, or mouse.</summary>
    /// <param name="sender">The event sender.</param>
    /// <param name="e">The event data.</param>
    private void OnButtonPressed(object sender, ButtonPressedEventArgs e) {
        // ignore if player hasn't loaded a save yet
        if (!Context.IsWorldReady)
            return;

        if (e.Button == SButton.J) {
            var magicalObject = new SObject(Vector2.Zero, MagicalObject.ObjectId);
            Game1.player.addItemByMenuIfNecessary(magicalObject);
        }

        if (e.Button == SButton.K) {
            Game1.activeClickableMenu = new ChallengeMenu();
        }
    }

    public override IChallengerApi GetApi() => _api;

    public IChallengerApi GetChallengerApi() => _api;
}