using Netcode;
using Newtonsoft.Json;
using StardewValley.Network;

namespace Slothsoft.Challenger.Models;

public sealed class ChallengerState : INetObject<NetFields> {
    public NetRef<ChallengeSelection> ChallengeSelection { get; } = new();

    public NetStringDictionary<EarnMoneyProgress, NetRef<EarnMoneyProgress>> EarnMoneyProgresses = new();

    public ChallengerState() {
        NetFields.AddFields(
            ChallengeSelection,
            EarnMoneyProgresses
        );
    }

    [JsonIgnore]
    public NetFields NetFields { get; } = new();
}