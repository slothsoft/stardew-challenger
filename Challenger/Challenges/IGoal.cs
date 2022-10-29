using Slothsoft.Challenger.Api;

namespace Slothsoft.Challenger.Challenges; 

public interface IGoal {
    /// <summary>
    /// Returns the display name of this goal.
    /// </summary>
    /// <returns></returns>
    string GetDisplayName(Difficulty difficulty);
    
    /// <summary>
    /// Returns if some progress was made in completing this goal.
    /// </summary>
    /// <returns></returns>
    bool WasStarted();
    /// <summary>
    /// Returns a string describing the progress that was made in completing this goal.
    /// </summary>
    /// <returns></returns>
    string GetProgress(Difficulty difficulty);
    /// <summary>
    /// Returns if this goal was reached.
    /// </summary>
    /// <returns></returns>
    bool IsCompleted(Difficulty difficulty);
    /// <summary>
    /// This starts tracking the goal.
    /// </summary>
    void Start();
    /// <summary>
    /// This stops tracking the goal.
    /// </summary>
    void Stop();
}