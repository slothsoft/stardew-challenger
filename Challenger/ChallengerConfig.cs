namespace Slothsoft.Challenger;

internal record ChallengerConfig {

    public SButton ButtonOpenMenu { get; set; } = SButton.K;
    public bool DisplayEarnMoneyChallenge { get; set; } = false;
}