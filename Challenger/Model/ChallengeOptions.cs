using System.Linq;
using Slothsoft.Challenger.Challenges;

namespace Slothsoft.Challenger.Model {
    
    public static class ChallengeOptions {

        private static IChallenge _activeChallenge = LoadActiveChallenge();
        
        private static IChallenge LoadActiveChallenge() {
            var dto = ModEntry.ModHelper.Data.ReadSaveData<ChallengeOptionsDto>(ChallengeOptionsDto.Key);
            var activeChallenge = ModEntry.AllChallenges.Single(c => c.Id == (dto?.ChallengeId ?? NoChallenge.ChallengeId));
            activeChallenge.ApplyRestrictions(ModEntry.ModHelper);
            return activeChallenge;
        }
        
        public static IChallenge GetActiveChallenge() {
            return _activeChallenge;
        }
        
        public static void SetActiveChallenge(IChallenge activeChallenge) {
            if (activeChallenge != _activeChallenge) {
                _activeChallenge.RemoveRestrictions(ModEntry.ModHelper);
                _activeChallenge = activeChallenge;
                ModEntry.ModHelper.Data.WriteSaveData(ChallengeOptionsDto.Key, new ChallengeOptionsDto(_activeChallenge.Id));
                _activeChallenge.ApplyRestrictions(ModEntry.ModHelper);
            }
        }
    }
    
    record ChallengeOptionsDto(string ChallengeId) {
        public const string Key = "ChallengeOptions";
    }
}