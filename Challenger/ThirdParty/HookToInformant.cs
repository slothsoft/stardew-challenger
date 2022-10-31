using Microsoft.Xna.Framework.Graphics;
using Slothsoft.Challenger.Challenges;
using Slothsoft.Challenger.Goals;

namespace Slothsoft.Challenger.ThirdParty; 

internal static class HookToInformant {
    
    private static Texture2D? _bundle;
    
    public static void Apply(ChallengerMod challengerMod) {
        // get Generic Mod Config Menu's API (if it's installed)
        var configMenu = challengerMod.Helper.ModRegistry.GetApi<IInformant>("Slothsoft.Informant");
        if (configMenu is null)
            return;

        _bundle ??= challengerMod.Helper.ModContent.Load<Texture2D>("assets/challenge_decorator.png");
        configMenu.AddItemDecorator(
            "aslkmklsad",
            "Trophy",
            "Bla bla",
            FetchDecoratorIfNecessary
        );
    }

    static Texture2D? FetchDecoratorIfNecessary(Item item) {
        // figure out if the item is used in the challenge
        var api = ChallengerMod.Instance.GetApi();
        if (api == null) {
            return null;
        }
        
        // only one goal currently has items that need to be decorated
        var goal = (api.ActiveChallenge as BaseChallenge)?.GetGoal() as EarnMoneyGoal;
        if (goal == null) {
            return null;
        }
        return goal.IsCountingAllowed(item) ? _bundle : null;
    }
}