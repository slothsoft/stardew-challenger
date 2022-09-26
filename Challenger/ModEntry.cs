using Slothsoft.Challenger.Challenges;
using Slothsoft.Challenger.Menu;
using Slothsoft.Challenger.Model;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;

namespace Slothsoft.Challenger
{
    public class ModEntry : Mod {

        // TODO: this should probably be private (and non-static) at best
        internal static IModHelper ModHelper;
        internal static readonly IChallenge[] AllChallenges = {
            new NoChallenge(),
            new NoCapitalistChallenge(),
        };

        private bool _challengeEnabled;

        /// <summary>The mod entry point, called after the mod is first loaded.</summary>
        /// <param name="newHelper">Provides simplified APIs for writing mods.</param>

        public override void Entry(IModHelper newHelper) {
            ModHelper = newHelper;

            Helper.Events.GameLoop.SaveLoaded += (_, _) => Monitor.Log($"{ChallengeOptions.GetActiveChallenge().GetDisplayName(ModHelper)} was activated.", LogLevel.Debug);
            Helper.Events.Input.ButtonPressed += OnButtonPressed;
        }

        /// <summary>Raised after the player presses a button on the keyboard, controller, or mouse.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event data.</param>
        private void OnButtonPressed(object sender, ButtonPressedEventArgs e)
        {
            // ignore if player hasn't loaded a save yet
            if (!Context.IsWorldReady)
                return;

            if (e.Button == SButton.K) {
                Game1.activeClickableMenu = new ChallengeMenu();
            }
        }
    }
}
