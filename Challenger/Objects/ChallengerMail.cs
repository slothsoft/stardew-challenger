using StardewModdingAPI.Events;

namespace Slothsoft.Challenger.Objects; 

public static class ChallengerMail {
    private const string MagicalObjectMail = "Slothsoft.Challenger/MagicalObjectMail";
    public const string GoalCompletedMail = "Slothsoft.Challenger/GoalCompletedMail";
    
    public static void InitAndSend() {
        var helper = ChallengerMod.Instance.Helper;
        helper.Events.Content.AssetRequested += OnAssetRequested;
        
        helper.Events.GameLoop.SaveLoaded += (_, _) => {
            if (!Game1.player.mailReceived.Contains(MagicalObjectMail)) {
                Game1.player.mailbox.Add(MagicalObjectMail);
                Game1.player.mailReceived.Add(MagicalObjectMail);
            }
        };
        helper.Events.GameLoop.DayStarted += (_, _) => {
            if (!Game1.player.mailReceived.Contains(GoalCompletedMail) && 
                ChallengerMod.Instance.GetApi()!.GetActiveChallenge().IsCompleted()) {
                Game1.player.mailbox.Add(GoalCompletedMail);
                Game1.player.mailReceived.Add(GoalCompletedMail);
            }
        };
    }

    private static void OnAssetRequested(object? sender, AssetRequestedEventArgs e) {
        if (e.Name.StartsWith("Data/mail")) {
            // See documentation: https://stardewcommunitywiki.com/Modding:Common_tasks#Mail_content
            e.Edit(
                asset => {
                    var data = asset.AsDictionary<string, string>().Data;
                    
                    var helper = ChallengerMod.Instance.Helper;
                    var hello = helper.Translation.Get("ChallengerMail.Hello", new { name = "@"});
                    var goodbye = helper.Translation.Get("ChallengerMail.Goodbye");
                    
                    var mailBody = helper.Translation.Get("ChallengerMail.MagicalObjectMail");
                    data.Add(
                        MagicalObjectMail,
                        $"{hello}^^{mailBody}^^{goodbye}^^%item bigobject {MagicalObject.ObjectId} %%"
                    );
                    
                    mailBody = helper.Translation.Get("ChallengerMail.GoalCompletedMail");
                    data.Add(
                        GoalCompletedMail,
                        $"{hello}^^{mailBody}^^{goodbye}"
                    );
                });
        }
    }
}