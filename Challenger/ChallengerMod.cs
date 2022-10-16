using Slothsoft.Challenger.Api;
using Slothsoft.Challenger.Menus;
using Slothsoft.Challenger.Objects;
using Slothsoft.Challenger.ThirdParty;
using StardewModdingAPI.Events;

namespace Slothsoft.Challenger;

// ReSharper disable once ClassNeverInstantiated.Global
public class ChallengerMod : Mod {
    public static ChallengerMod Instance = null!;

    private IChallengerApi? _api;
    internal ChallengerConfig Config = null!;

    /// <summary>The mod entry point, called after the mod is first loaded.</summary>
    /// <param name="modHelper">Provides simplified APIs for writing mods.</param>
    public override void Entry(IModHelper modHelper) {
        Instance = this;
        Config = Helper.ReadConfig<ChallengerConfig>();

        Helper.Events.GameLoop.SaveLoaded += (_, _) => {
            _api = new ChallengerApi(modHelper);
            Monitor.Log($"Challenge \"{_api.GetActiveChallenge().GetDisplayName()}\" was activated.", LogLevel.Debug);
        };
        Helper.Events.Input.ButtonPressed += OnButtonPressed;
        Helper.Events.GameLoop.GameLaunched += OnGameLaunched;

        
        // Patches
        MagicalObject.PatchObject(ModManifest.UniqueID);
        ChallengerMail.InitAndSend();
    }

    /// <summary>Raised after the player presses a button on the keyboard, controller, or mouse.</summary>
    /// <param name="sender">The event sender.</param>
    /// <param name="e">The event data.</param>
    private void OnButtonPressed(object? sender, ButtonPressedEventArgs e) {
        // ignore if player hasn't loaded a save yet
        if (!Context.IsWorldReady)
            return;

        if (e.Button == Config.ButtonOpenMenu) {
            Game1.activeClickableMenu = new ChallengeMenu();
        }
    }

    public override IChallengerApi? GetApi() => _api;

    public bool IsInitialized() => _api != null;

    private void OnGameLaunched(object? sender, GameLaunchedEventArgs e) {
        HookToGenericModConfigMenu.Apply(this);
    }
}