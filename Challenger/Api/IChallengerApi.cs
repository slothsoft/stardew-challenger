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
    IChallenge ActiveChallenge { get; set; }
    
    /// <summary>
    /// Returns the currently active difficulty.
    /// </summary>
    Difficulty ActiveDifficulty { get; set; }

    /// <summary>
    /// Returns the currently active Magical Object Replacement ("active" means <see cref="ActiveChallenge"/> and <see cref="ActiveDifficulty"/>).
    /// </summary>
    MagicalReplacement ActiveChallengeMagicalReplacement => ActiveChallenge.GetMagicalReplacement(ActiveDifficulty);
    /// <summary>
    /// Returns the currently active Magical Object Replacement ("active" means <see cref="ActiveChallenge"/> and <see cref="ActiveDifficulty"/>).
    /// </summary>
    bool IsActiveChallengeCompleted => ActiveChallenge.IsCompleted(ActiveDifficulty);


}