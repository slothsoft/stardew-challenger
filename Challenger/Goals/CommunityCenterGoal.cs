using System.Linq;
using Slothsoft.Challenger.Api;

namespace Slothsoft.Challenger.Goals;

public class CommunityCenterGoal : IGoal {

    private IModHelper _modHelper;
    
    public CommunityCenterGoal(IModHelper modHelper) {
        _modHelper = modHelper;
    }

    public string GetDisplayName() {
        return _modHelper.Translation.Get(GetType().Name);
    }
    
    public bool IsCompleted() {
        return Game1.netWorldState.Value.Bundles.Values.SelectMany(b => b).All(v => v);
    }
    
    public void Start() {
        // we don't need to start this
    }

    public void Stop() {
        // we don't need to stop this
    }

    public bool WasStarted() {
        return true;
    }

    public string GetProgress() {
        int finished, all;
        
        if (Game1.player.hasCompletedCommunityCenter()) {
            // For some reason this shows as 98/100
            finished = 1;
            all = 1;
        } else if (Game1.player.hasOrWillReceiveMail("JojaMember")) {
            // Player is contributing to JojaMart
            var mailsReceived = GetMailsForCommunityCenter();
            finished = mailsReceived.Count(b => b);
            all = mailsReceived.Length;
        } else {
            // The "normal" community center
            finished = Game1.netWorldState.Value.Bundles.Values.SelectMany(b => b).Count(v => v);
            all = Game1.netWorldState.Value.Bundles.Values.SelectMany(b => b).Count();
        }
        return $"{(100 * finished / all):0} / 100%";
    }

    private static bool[] GetMailsForCommunityCenter() {
        // See Game1.player.hasCompletedCommunityCenter()
        return new[] {
            Game1.player.mailReceived.Contains("ccBoilerRoom"),
            Game1.player.mailReceived.Contains("ccCraftsRoom"),
            Game1.player.mailReceived.Contains("ccPantry"),
            Game1.player.mailReceived.Contains("ccFishTank"),
            Game1.player.mailReceived.Contains("ccVault"),
        };
    }
}