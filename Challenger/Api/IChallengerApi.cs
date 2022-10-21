using System;
using System.Collections.Generic;

namespace Slothsoft.Challenger.Api;

public interface IChallengerApi : IDisposable {
    /// <summary>
    /// Returns all challenges that are registered in this mod.
    /// </summary>
    /// <returns></returns>
    IEnumerable<IChallenge> GetAllChallenges();

    /// <summary>
    /// Returns the currently active challenge. Note that this value is never null,
    /// but instance of the class "NoChallenge" at worst.
    /// </summary>
    /// <returns></returns>
    IChallenge GetActiveChallenge();

    /// <summary>
    /// Sets the currently active challenge. Note that this value cannot be null;
    /// use instance of the class "NoChallenge" if you want to set no challenge.
    /// </summary>
    /// <returns></returns>
    void SetActiveChallenge(IChallenge activeChallenge);
}