namespace Slothsoft.Challenger.Api;

public interface IChallenge {
    /// <summary>
    /// An ID that never ever changes, so we can be sure to identify challenges.
    /// </summary>
    string Id { get; }

    /// <summary>
    /// Returns the display name of this challenge.
    /// </summary>
    /// <returns></returns>
    string GetDisplayName();

    /// <summary>
    /// Returns a string explaining what you can or cannot do in this challenge.
    /// </summary>
    /// <returns></returns>
    string GetDisplayText();

    /// <summary>
    /// This applies all restriction this challenge has to the game and starts tracking the goal.
    /// </summary>
    void Start();

    /// <summary>
    /// This removes all restriction this challenge has from the game and stops tracking the goal.
    /// </summary>
    void Stop();

    /// <summary>
    /// Returns the object you wish to replace the magical object with.
    /// </summary>
    /// <returns>replacement object.</returns>
    MagicalReplacement GetMagicalReplacement();
    
    /// <summary>
    /// Returns the goal that should be reached.
    /// </summary>
    /// <returns>the goal.</returns>
    IGoal GetGoal();

    /// <summary>
    /// Returns if the challenge is completed. 
    /// </summary>
    /// <returns></returns>
    bool IsCompleted();
}