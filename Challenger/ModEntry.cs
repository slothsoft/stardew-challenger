using Slothsoft.Challenger.Challenges;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;

namespace Slothsoft.Challenger
{
    public class ModEntry : Mod
    {
        private const string ModSparkle = "assets/sparkle.png";

        private IModHelper _helper;
        private IChallenge _challenge;
        private bool _challengeEnabled;

        /// <summary>The mod entry point, called after the mod is first loaded.</summary>
        /// <param name="newHelper">Provides simplified APIs for writing mods.</param>

        public override void Entry(IModHelper newHelper)
        {
            _helper = newHelper;

            _challenge = new NoCapitalist();
            ToggleEnablement();
            _helper.Events.Input.ButtonPressed += OnButtonPressed;
        }

        private void ToggleEnablement() {
            Monitor.Log($"{_challenge.GetDisplayName(_helper)} was enabled {_challengeEnabled}.", LogLevel.Debug);
            Monitor.Log($"{_challenge.GetDisplayText(_helper)}", LogLevel.Debug);
            if (_challengeEnabled) {  ;
                _challenge.RemoveRestrictions(_helper);
                _challengeEnabled = false;
            } else {
                _challenge.ApplyRestrictions(_helper);    
                _challengeEnabled = true;
            }
        }

        /// <summary>Raised after the player presses a button on the keyboard, controller, or mouse.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event data.</param>
        private void OnButtonPressed(object sender, ButtonPressedEventArgs e)
        {
            // ignore if player hasn't loaded a save yet
            if (!Context.IsWorldReady)
                return;

            if (e.Button == SButton.J) {
                ToggleEnablement();
            }
        }
    }
}
