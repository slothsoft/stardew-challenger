using Netcode;
using Newtonsoft.Json;
using Slothsoft.Challenger.Api;

namespace Slothsoft.Challenger.Models;

public record ChallengeSelection : INetObject<NetFields> {
    
    public const string Key = "ChallengerSaveDto";
    
    public ChallengeSelection() {
        NetFields.AddFields(
            _challengeId,
            _difficulty
        );
    }
    
    public ChallengeSelection(string challengeId, Difficulty difficulty) : this() {
        _challengeId.Value = challengeId;
        _difficulty.Value = difficulty;
    }

    [JsonIgnore]
    public NetFields NetFields { get; } = new();

    private readonly NetString _challengeId = new();
    public string ChallengeId {
        get => _challengeId.Value;
        set => _challengeId.Value = value;
    }

    private readonly NetEnum<Difficulty> _difficulty = new();
    public Difficulty Difficulty {
        get => _difficulty.Value;
        set => _difficulty.Value = value;
    }
}