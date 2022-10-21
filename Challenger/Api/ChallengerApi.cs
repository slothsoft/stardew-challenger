﻿using System;
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
            new VineyardChallenge(modHelper),
        };
        _challenges.Sort((a, b) =>
            string.Compare(a.GetDisplayName(), b.GetDisplayName(), StringComparison.CurrentCulture));
        _challenges.Insert(0, new NoChallenge(modHelper));

        _activeChallenge = LoadActiveChallenge();
    }

    private IChallenge LoadActiveChallenge() {
        var dto = _modHelper.Data.ReadSaveData<ChallengerSaveDto>(ChallengerSaveDto.Key);
        var challengeId = dto?.ChallengeId ?? NoChallenge.ChallengeId;
        
        var activeChallenge = _challenges.SingleOrDefault(c => c.Id == challengeId);
        if (activeChallenge == null) {
            // this can happen if a challenge ID changed or a challenge was removed
            ChallengerMod.Instance.Monitor.Log($"Challenge \"{challengeId}\" was not found.", LogLevel.Debug);
            activeChallenge = new NoChallenge(_modHelper);
        }
        activeChallenge.Start();
        return activeChallenge;
    }

    public IEnumerable<IChallenge> GetAllChallenges() {
        return _challenges.ToImmutableArray();
    }

    public IChallenge GetActiveChallenge() {
        return _activeChallenge;
    }

    public void SetActiveChallenge(IChallenge activeChallenge) {
        if (activeChallenge != _activeChallenge) {
            _activeChallenge.Stop();
            ChallengerMod.Instance.Monitor.Log($"Challenge \"{_activeChallenge.GetDisplayName()}\" was stopped.",
                LogLevel.Debug);

            _activeChallenge = activeChallenge;
            _modHelper.Data.WriteSaveData(ChallengerSaveDto.Key, new ChallengerSaveDto(_activeChallenge.Id));

            _activeChallenge.Start();
            ChallengerMod.Instance.Monitor.Log($"Challenge \"{_activeChallenge.GetDisplayName()}\" was started.",
                LogLevel.Debug);
        }
    }

    public void Dispose() {
        _activeChallenge.Stop();
    }
}

internal record ChallengerSaveDto(string ChallengeId) {
    public const string Key = "ChallengerSaveDto";
}