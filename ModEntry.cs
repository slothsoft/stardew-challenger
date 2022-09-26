using Slothsoft.Challenger.Challenges;
using StardewModdingAPI;

namespace Slothsoft.Challenger
{
    public class ModEntry : Mod
    {
        private const string ModSparkle = "assets/sparkle.png";

        private IModHelper _helper;

        /// <summary>The mod entry point, called after the mod is first loaded.</summary>
        /// <param name="newHelper">Provides simplified APIs for writing mods.</param>

        public override void Entry(IModHelper newHelper)
        {
            _helper = newHelper;
            
            new NoCapitalist().ApplyRestrictions(newHelper);
        }
    }
}
