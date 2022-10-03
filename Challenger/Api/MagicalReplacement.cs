using Slothsoft.Challenger.Objects;

namespace Slothsoft.Challenger.Api;

/// <summary>
/// Record for what to replace the magical object with.
/// </summary>
public record MagicalReplacement(
    // See https://stardewcommunitywiki.com/Modding:Big_craftables_data
    int ParentSheetIndex,
    string Name
) {
    public static readonly MagicalReplacement Default = new(107, MagicalObject.ObjectName); // Pink bunny 
    
    public static readonly MagicalReplacement Keg = new(12, "Keg"); 
    public static readonly MagicalReplacement SeedMaker = new(25, "Seed Maker"); 
}