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
    /// This applies all restriction this challenge has to the game.
    /// </summary>
    void ApplyRestrictions();

    /// <summary>
    /// This removes all restriction this challenge has from the game.
    /// </summary>
    void RemoveRestrictions();
}