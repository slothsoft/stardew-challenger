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

    public ChallengerApi(IModHelper modHelper) {
        _modHelper = modHelper;

        _challenges = new List<IChallenge> {
            new BreweryChallenge(modHelper),
            new HermitChallenge(modHelper),
            new NoCapitalistChallenge(modHelper),
            new VinyardChallenge(modHelper),
        };
        _challenges.Sort((a, b) =>
            String.Compare(a.GetDisplayName(), b.GetDisplayName(), StringComparison.CurrentCulture));
        _challenges.Insert(0, new NoChallenge(modHelper));

        _activeChallenge = LoadActiveChallenge();
    }

    private IChallenge LoadActiveChallenge() {
        var dto = _modHelper.Data.ReadSaveData<ChallengerSaveDto>(ChallengerSaveDto.Key);
        var activeChallenge = _challenges.Single(c => c.Id == (dto?.ChallengeId ?? NoChallenge.ChallengeId));
        activeChallenge.ApplyRestrictions();
        return activeChallenge;
    }

    public IReadOnlyCollection<IChallenge> GetAllChallenges() {
        return _challenges.ToImmutableArray();
    }

    public IChallenge GetActiveChallenge() {
        return _activeChallenge;
    }

    public void SetActiveChallenge(IChallenge activeChallenge) {
        if (activeChallenge != _activeChallenge) {
            _activeChallenge.RemoveRestrictions();
            ChallengerMod.Instance.Monitor.Log($"Challenge \"{_activeChallenge.GetDisplayName()}\" was ended.",
                LogLevel.Debug);

            _activeChallenge = activeChallenge;
            _modHelper.Data.WriteSaveData(ChallengerSaveDto.Key, new ChallengerSaveDto(_activeChallenge.Id));

            _activeChallenge.ApplyRestrictions();
            ChallengerMod.Instance.Monitor.Log($"Challenge \"{_activeChallenge.GetDisplayName()}\" was activated.",
                LogLevel.Debug);
        }
    }
}

internal record ChallengerSaveDto(string ChallengeId) {
    public const string Key = "ChallengerSaveDto";
}