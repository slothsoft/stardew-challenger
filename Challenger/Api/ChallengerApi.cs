using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Slothsoft.Challenger.Challenges;

namespace Slothsoft.Challenger.Api;

internal class ChallengerApi : IChallengerApi {
    
    private readonly List<IChallenge> _challenges;
    private readonly IModHelper _modHelper;
    
    private IChallenge _activeChallenge;
    private Difficulty _activeDifficulty;

    public ChallengerApi(IModHelper modHelper) {
        _modHelper = modHelper;

        _challenges = new List<IChallenge> {
            new BreweryChallenge(modHelper),
            new HermitChallenge(modHelper),
            new NoCapitalistChallenge(modHelper),
            new VineyardChallenge(modHelper),
        };
        _challenges.Sort((a, b) => string.Compare(a.DisplayName, b.DisplayName, StringComparison.CurrentCulture));
        _challenges.Insert(0, new NoChallenge(modHelper));

        _activeChallenge = LoadActiveChallenge();
    }

    private IChallenge LoadActiveChallenge() {
        var dto = _modHelper.Data.ReadSaveData<ChallengerSaveDto>(ChallengerSaveDto.Key);
        var challengeId = dto?.ChallengeId ?? NoChallenge.ChallengeId;
        _activeDifficulty = dto?.Difficulty ?? Difficulty.Medium;
        
        var activeChallenge = _challenges.SingleOrDefault(c => c.Id == challengeId);
        if (activeChallenge == null) {
            // this can happen if a challenge ID changed or a challenge was removed
            ChallengerMod.Instance.Monitor.Log($"Challenge \"{challengeId}\" was not found.", LogLevel.Debug);
            activeChallenge = new NoChallenge(_modHelper);
        }
        activeChallenge.Start(_activeDifficulty);
        return activeChallenge;
    }

    public IEnumerable<IChallenge> GetAllChallenges() {
        return _challenges.ToImmutableArray();
    }

    public IChallenge GetActiveChallenge() {
        return _activeChallenge;
    }

    public Difficulty ActiveDifficulty {
        get => _activeDifficulty;
        set {
            if (value != _activeDifficulty) {
                UpdateChallengeAndDifficulty(_activeChallenge, value);
            }
        }
    }
    
    public IChallenge ActiveChallenge {
        get => _activeChallenge;
        set {
            if (value != _activeChallenge) {
                UpdateChallengeAndDifficulty(value, _activeDifficulty);
            }
        }
    }

    private void UpdateChallengeAndDifficulty(IChallenge newActiveChallenge, Difficulty newDifficulty) {
        _activeChallenge.Stop();
        ChallengerMod.Instance.Monitor.Log($"Challenge \"{_activeChallenge.DisplayName}\" was stopped.",
            LogLevel.Debug);

        _activeChallenge = newActiveChallenge;
        _activeDifficulty = newDifficulty;
        _modHelper.Data.WriteSaveData(ChallengerSaveDto.Key, new ChallengerSaveDto(_activeChallenge.Id, _activeDifficulty));

        _activeChallenge.Start(newDifficulty);
        ChallengerMod.Instance.Monitor.Log($"Challenge \"{_activeChallenge.DisplayName} ({newDifficulty})\" was started.",
            LogLevel.Debug);
    }

    public void Dispose() {
        _activeChallenge.Stop();
    }
}

internal record ChallengerSaveDto(string ChallengeId, Difficulty Difficulty) {
    public const string Key = "ChallengerSaveDto";
}