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

        internal static ChallengeOptions LoadChallengeOptions() {
            return ModHelper.Data.ReadSaveData<ChallengeOptions>(ChallengeOptions.Key);
        }
        
        internal static void SaveChallengeOptions(ChallengeOptions options) {
            ModHelper.Data.WriteSaveData(ChallengeOptions.Key, options);
        }
        
        private IChallenge _challenge;
        private bool _challengeEnabled;

        /// <summary>The mod entry point, called after the mod is first loaded.</summary>
        /// <param name="newHelper">Provides simplified APIs for writing mods.</param>

        public override void Entry(IModHelper newHelper) {
            ModHelper = newHelper;
            
            _challenge = AllChallenges[0];
            ToggleEnablement();
            Helper.Events.Input.ButtonPressed += OnButtonPressed;
        }

        private void ToggleEnablement() {
            Monitor.Log($"{_challenge.GetDisplayName(Helper)} was enabled {_challengeEnabled}.", LogLevel.Debug);
            Monitor.Log($"{_challenge.GetDisplayText(Helper)}", LogLevel.Debug);
            if (_challengeEnabled) {  ;
                _challenge.RemoveRestrictions(Helper);
                _challengeEnabled = false;
            } else {
                _challenge.ApplyRestrictions(Helper);    
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
            if (e.Button == SButton.K) {
                Game1.activeClickableMenu = new ChallengeMenu();
            }
        }
    }
}
