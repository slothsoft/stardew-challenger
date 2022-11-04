using Slothsoft.Challenger.Api;
using Slothsoft.Challenger.Common;
using Slothsoft.Challenger.Events;

// ReSharper disable InconsistentNaming

namespace Slothsoft.Challenger.Restrictions;

public class ExcludeGlobalSell : IRestriction {
    
    private readonly string _displayName;
    private readonly int[] _allowedCategories;

    public ExcludeGlobalSell(string displayName,  params int[] allowedCategories) {
        _displayName = displayName;
        _allowedCategories = allowedCategories;
    }

    public string GetDisplayText() {
        return CommonHelpers.ToListString(_displayName);
    }

    public void Apply() {
        GlobalSellChanger.AddAllowedCategories(_allowedCategories);
    }

    public void Remove() {
        GlobalSellChanger.RemoveAllowedCategories(_allowedCategories);
    }
}