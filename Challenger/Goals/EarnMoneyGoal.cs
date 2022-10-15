using System;
using Slothsoft.Challenger.Events;

namespace Slothsoft.Challenger.Goals;

public class EarnMoneyGoal : BaseGoal<EarnMoneyProgress> {
    
    private readonly int _targetMoney;
    private readonly Func<Item, bool> _isCountingAllowed;
    private readonly string _displayNameKey;

    public EarnMoneyGoal(IModHelper modHelper, int targetMoney) : base(modHelper, "earn-money") {
        _targetMoney = targetMoney;
        _isCountingAllowed = _ => true;
        _displayNameKey = GetType().Name;
    }
    
    public EarnMoneyGoal(IModHelper modHelper, int targetMoney, string suffix, Func<Item, bool> isCountingAllowed) : base(modHelper, $"earn-money-{suffix}") {
        _targetMoney = targetMoney;
        _isCountingAllowed = isCountingAllowed;
        _displayNameKey = GetType().Name + "." + suffix;
    }

    public override string GetDisplayName() {
        return ModHelper.Translation.Get(_displayNameKey, new {
            Value = StringifyCurrency(_targetMoney),
        });
    }
    
    private static string StringifyCurrency(int value) {
        return $"$ {value:N0}";
    }
    
    public override void Start() {
        GlobalMoneyCounter.AddSellEvent(OnItemSold);
    }
    
    private void OnItemSold(Item soldItem, int moneyEarned) {
        if (_isCountingAllowed(soldItem)) {
            Progress.MoneyEarned += moneyEarned;
            WriteProgressType(Progress);
        }
    }

    public override void Stop() {
        GlobalMoneyCounter.RemoveSellEvent(OnItemSold);
    }

    public override bool WasStarted() {
        return Progress.MoneyEarned > 0;
    }

    public override string GetProgress() {
        return $"{StringifyCurrency(Progress.MoneyEarned)} / {StringifyCurrency(_targetMoney)}";
    }

    public override bool IsCompleted() {
        return Progress.MoneyEarned >= _targetMoney;
    }
}

public record EarnMoneyProgress {

    public int MoneyEarned { get; set; }
}