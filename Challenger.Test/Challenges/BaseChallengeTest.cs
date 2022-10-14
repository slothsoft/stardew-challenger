using NUnit.Framework;
using Slothsoft.Challenger.Api;

namespace ChallengerTest.Challenges; 

internal class BaseChallengeTest {
    
    [SetUp]
    public void Setup() {
    }

    [Test]
    public void Test() {
        // neither of these lines work:
        
        // IModHelper modHelper = null;
        // var modHelper = new Mock<IModHelper>();
        // ChallengerMod challengerMod = null;
        
        // but these do:
        
        // var modHelper = "test";
        // var modHelper = typeof(IGenericModConfigMenuApi);
        // IGenericModConfigMenuApi modHelper = null;
        IChallenge modHelper = null;
        Assert.IsNull(modHelper);
    }
}