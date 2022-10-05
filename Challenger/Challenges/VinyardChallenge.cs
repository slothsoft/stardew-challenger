using System.Collections.Generic;
using Slothsoft.Challenger.Api;
using Slothsoft.Challenger.Models;
using Slothsoft.Challenger.Restrictions;

namespace Slothsoft.Challenger.Challenges;

public class VinyardChallenge : BaseChallenge {
    public VinyardChallenge(IModHelper modHelper) : base(modHelper, "vinyard") {
    }

    protected override IRestriction[] CreateRestrictions(IModHelper modHelper) {
        return new IRestriction[] {
            new RenameVanillaObject(modHelper, new Dictionary<RenameVanillaObject.VanillaObject, string>{
                    { new(ObjectIds.Juice, SObject.PreserveType.Juice, ObjectIds.UnmilledRice), 
                        modHelper.Translation.Get("VinyardChallenge.RenameRiceJuice") },
            }),
        };
    }
}