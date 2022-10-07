using Slothsoft.Challenger.Api;
using Slothsoft.Challenger.Models;

// ReSharper disable InconsistentNaming

namespace Slothsoft.Challenger.Restrictions;

public class ExcludeGlobalCarpenter : IRestriction {
    
    private readonly string _displayName;
    private readonly string[] _excludedBluePrintNames;

    public ExcludeGlobalCarpenter(string displayName,  params string[] excludedBluePrintNames) {
        _displayName = displayName;
        _excludedBluePrintNames = excludedBluePrintNames;
    }

    public string GetDisplayText() {
        return CommonHelpers.ToListString(_displayName);
    }

    public void Apply() {
        GlobalCarpenterChanger.AddExcludedBluePrintNames(_excludedBluePrintNames);
    }

    public void Remove() {
        GlobalCarpenterChanger.RemoveExcludedBluePrintNames(_excludedBluePrintNames);
    }
}