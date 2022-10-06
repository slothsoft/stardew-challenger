using System;
using System.Collections.Generic;
using Slothsoft.Challenger.Api;
using Slothsoft.Challenger.Models;

// ReSharper disable InconsistentNaming

namespace Slothsoft.Challenger.Restrictions;

public class ExcludeGlobalStock : IRestriction {
    
    private readonly string _displayName;
    private readonly Func<ISalable, bool> _excludeFilter;

    public ExcludeGlobalStock(string displayName,  Func<ISalable, bool> excludeFilter) {
        _displayName = displayName;
        _excludeFilter = excludeFilter;
    }

    public string GetDisplayText() {
        return CommonHelpers.ToListString(_displayName);
    }

    public void Apply() {
        GlobalStockChanger.AddChanger(ChangeShopStock);
    }

    private void ChangeShopStock(IDictionary<ISalable, int[]> stock) {
        foreach (var keyValuePair in stock) {
            if (_excludeFilter.Invoke(keyValuePair.Key)) {
                stock.Remove(keyValuePair.Key);
            }
        }
    }

    public void Remove() {
        GlobalStockChanger.RemoveChanger(ChangeShopStock);
    }
}