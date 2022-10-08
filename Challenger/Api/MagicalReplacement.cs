using Slothsoft.Challenger.Models;
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
    public static readonly MagicalReplacement Default = new(ObjectIds.PinkyBunny, MagicalObject.ObjectName); // Pink bunny 
    
    public static readonly MagicalReplacement Keg = new(ObjectIds.Keg, "Keg"); 
    public static readonly MagicalReplacement SeedMaker = new(ObjectIds.SeedMaker, "Seed Maker"); 
}