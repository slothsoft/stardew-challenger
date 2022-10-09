using System.Collections.Generic;
using System.Linq;
using Slothsoft.Challenger.Api;
using Slothsoft.Challenger.Models;
using Slothsoft.Challenger.Restrictions;

namespace Slothsoft.Challenger.Challenges;

public class BreweryChallenge : BaseChallenge {
    public BreweryChallenge(IModHelper modHelper) : base(modHelper, "brewery") {
    }

    protected override IRestriction[] CreateRestrictions(IModHelper modHelper) {
        return new[] {
            CreateRenameRiceJuice(modHelper),
            CreateIncludeFruitOnly(modHelper),
            VinyardChallenge.CreateExcludeAnimalBuildings(modHelper),
        };
    }
    
    private static IRestriction CreateRenameRiceJuice(IModHelper modHelper) {
        return new RenameVanillaObject(
            modHelper,
            new RenameVanillaObject.VanillaObject(ObjectIds.Juice, SObject.PreserveType.Juice, ObjectIds.UnmilledRice),
            modHelper.Translation.Get("BreweryChallenge.RenameRiceJuice")
        );
    }

    private static IRestriction CreateIncludeFruitOnly(IModHelper modHelper) {
        var beerSeeds = new [] {
            SeedIds.Rice, SeedIds.Hops, SeedIds.Wheat
        };
        return new ExcludeGlobalStock(modHelper.Translation.Get("BreweryChallenge.IncludeBeerVegetablesOnly"), s => {
            // everything that is not a basic object is allowed
            if (s is not SObject obj) return false;
            // everything that is a sapling is not allowed
            if (SaplingIds.AllSaplings.Contains(obj.ParentSheetIndex)) return true;
            // everything that is not a seed is allowed
            if (!SeedIds.AllSeeds.Contains(obj.ParentSheetIndex)) return false;
            // everything that is a seed for beer is allowed
            if (beerSeeds.Contains(obj.ParentSheetIndex)) return false;
            // and we won't allow anything else
            return true;
        });
    }
    
    public override MagicalReplacement GetMagicalReplacement() {
        return MagicalReplacement.Keg;
    }
}