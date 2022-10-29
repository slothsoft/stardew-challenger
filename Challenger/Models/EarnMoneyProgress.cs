using Netcode;
using Newtonsoft.Json;

namespace Slothsoft.Challenger.Models;

public record EarnMoneyProgress : INetObject<NetFields> {
    
    public EarnMoneyProgress() {
        NetFields.AddFields(
            _moneyEarned
        );
    }

    [JsonIgnore]
    public NetFields NetFields { get; } = new();

    private readonly NetInt _moneyEarned = new();
    public int MoneyEarned {
        get => _moneyEarned.Value;
        set => _moneyEarned.Value = value;
    }
}