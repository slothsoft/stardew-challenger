using Pathoschild.Stardew.Automate;
using StardewModdingAPI.Events;

namespace Slothsoft.ChallengerAutomate;

/// <summary>
/// This hooks the Automate mod to the Challenger.
///
/// Documentation: https://github.com/Pathoschild/StardewMods/blob/develop/Automate/docs/technical.md#extensibility-for-modders
/// </summary>
// ReSharper disable once ClassNeverInstantiated.Global
public class ChallengerAutomateMod : Mod {
    internal static ChallengerAutomateMod Instance = null!;

    /// <summary>The mod entry point, called after the mod is first loaded.</summary>
    /// <param name="modHelper">Provides simplified APIs for writing mods.</param>
    public override void Entry(IModHelper modHelper) {
        Instance = this;
        Helper.Events.GameLoop.GameLaunched += OnGameLaunched;
    }
    
    private void OnGameLaunched(object? sender, GameLaunchedEventArgs e) {
        var automate = Helper.ModRegistry.GetApi<IAutomateAPI>("Pathoschild.Automate");
        automate!.AddFactory(new MagicalObjectAutomationFactory());
    }
}