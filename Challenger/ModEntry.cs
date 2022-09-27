using Slothsoft.Challenger.Api;
using Slothsoft.Challenger.Menu;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;

namespace Slothsoft.Challenger {
    public class ModEntry : Mod {
        internal static ModEntry Instance;

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
        }

        /// <summary>Raised after the player presses a button on the keyboard, controller, or mouse.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event data.</param>
        private void OnButtonPressed(object sender, ButtonPressedEventArgs e) {
            // ignore if player hasn't loaded a save yet
            if (!Context.IsWorldReady)
                return;

            if (e.Button == SButton.K) {
                Game1.activeClickableMenu = new ChallengeMenu();
            }
        }

        public override object GetApi() => _api;

        public IChallengerApi GetChallengerApi() => _api;
    }
}