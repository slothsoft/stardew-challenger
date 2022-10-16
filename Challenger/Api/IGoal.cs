namespace Slothsoft.Challenger.Api; 

public interface IGoal {
    /// <summary>
    /// Returns the display name of this goal.
    /// </summary>
    /// <returns></returns>
    string GetDisplayName();
    /// <summary>
    /// Returns if some progress was made in completing this goal.
    /// </summary>
    /// <returns></returns>
    bool WasStarted();
    /// <summary>
    /// Returns a string describing the progress that was made in completing this goal.
    /// </summary>
    /// <returns></returns>
    string GetProgress();
    /// <summary>
    /// Returns if this goal was reached.
    /// </summary>
    /// <returns></returns>
    bool IsCompleted();
    /// <summary>
    /// This starts tracking the goal.
    /// </summary>
    void Start();
    /// <summary>
    /// This stops tracking the goal.
    /// </summary>
    void Stop();
}