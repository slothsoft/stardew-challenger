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

        /// <summary>The mod entry point, called after the mod is first loaded.</summary>
        /// <param name="newHelper">Provides simplified APIs for writing mods.</param>

        public override void Entry(IModHelper newHelper)
        {
            _helper = newHelper;

            _challenge = new NoCapitalist();
            _challenge.ApplyRestrictions(newHelper);      
            _helper.Events.Input.ButtonPressed += OnButtonPressed;
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
                // print button presses to the console window
                Monitor.Log($"{Game1.player.Name} pressed {e.Button}.", LogLevel.Debug);
                _challenge.RemoveRestrictions(_helper);      
                // Game1.addHUDMessage(new HUDMessage("YOU CANNOT DO THIS FOR THIS CHALLENGE", HUDMessage.error_type));
            }
        }
    }
}
