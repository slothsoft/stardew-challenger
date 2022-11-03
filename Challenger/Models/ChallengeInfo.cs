using Netcode;
using Newtonsoft.Json;
using Slothsoft.Challenger.Api;
using StardewValley.Network;

namespace Slothsoft.Challenger.Models;

public record ChallengeInfo : INetObject<NetFields> {
    
    public ChallengeInfo() {
        NetFields.AddFields(
            _startedOn,
            CompletedOn
        );
    }

    [JsonIgnore]
    public NetFields NetFields { get; } = new();

    private readonly NetInt _startedOn = new(-1);
    public int? StartedOn {
        get => _startedOn.Value < 0 ? null : _startedOn.Value;
        set => _startedOn.Value = value ?? -1;
    }
    
    public NetIntDictionary<int, NetInt> CompletedOn { get; } = new();

    public WorldDate? GetCompletedOnDate(Difficulty difficulty) {
        if (CompletedOn.ContainsKey((int) difficulty)) {
            var completedOn = CompletedOn[(int) difficulty];
            if (completedOn < -1) {
                return null;
            }
            return new WorldDate {
                TotalDays = completedOn
            };
        }
        return null; 
    }
    
    public void SetCompletedOnDate(Difficulty difficulty, WorldDate? date) {
        CompletedOn[(int) difficulty] = date?.TotalDays ?? -1;
    }
}