using System.Collections.Generic;
using System.Linq;
using Slothsoft.Challenger.Api;
using Slothsoft.Challenger.Models;
using Slothsoft.Challenger.Restrictions;

namespace Slothsoft.Challenger.Challenges;

public class VinyardChallenge : BaseChallenge {
    public VinyardChallenge(IModHelper modHelper) : base(modHelper, "vinyard") {
    }

    protected override IRestriction[] CreateRestrictions(IModHelper modHelper) {
        return new[] {
            CreateRenameRiceJuice(modHelper),
            CreateIncludeFruitOnly(modHelper),
            CreateExcludeAnimalBuildings(modHelper),
            ChangeGlobalStock.AddRiceInFirstSpring(modHelper),
        };
    }

    private static IRestriction CreateRenameRiceJuice(IModHelper modHelper) {
        return new RenameVanillaObject(
            modHelper,
            new RenameVanillaObject.VanillaObject(ObjectIds.Juice, SObject.PreserveType.Juice, ObjectIds.UnmilledRice),
            modHelper.Translation.Get("VinyardChallenge.RenameRiceJuice")
        );
    }

    private static IRestriction CreateIncludeFruitOnly(IModHelper modHelper) {
        var fruitIds = new [] {
            SeedIds.Strawberry, SeedIds.Rice, SeedIds.Blueberry, SeedIds.Melon, SeedIds.Starfruit, SeedIds.Cranberries, 
            SeedIds.Grape, SeedIds.AncientFruit, SeedIds.CactusFruit, SeedIds.Pineapple, SeedIds.QiFruit, 
            SeedIds.SweetGemBerry,
        };
        return ChangeGlobalStock.ExcludeSalables(modHelper.Translation.Get("VinyardChallenge.IncludeFruitOnly"), s => {
            // everything that is not a basic object is allowed
            if (s is not SObject obj) return false;
            // everything that is not a seed is allowed
            if (!SeedIds.AllSeeds.Contains(obj.ParentSheetIndex)) return false;
            // everything that is a seed for a fruit or rice is allowed
            if (fruitIds.Contains(obj.ParentSheetIndex)) return false;
            // and we won't allow anything else
            return true;
        });
    }

    internal static IRestriction CreateExcludeAnimalBuildings(IModHelper modHelper) {
        return new ExcludeGlobalCarpenter(modHelper.Translation.Get("VinyardChallenge.ExcludeAnimalBuildings"),  BluePrintNames.Barn, BluePrintNames.Silo, BluePrintNames.Coop);
    }
    
    public override MagicalReplacement GetMagicalReplacement() {
        return MagicalReplacement.Keg;
    }
}