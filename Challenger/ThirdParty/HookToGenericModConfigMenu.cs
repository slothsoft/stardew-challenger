namespace Slothsoft.Challenger.ThirdParty; 

public static class HookToGenericModConfigMenu {
    
    public static void Apply(ChallengerMod challengerMod) {
        // get Generic Mod Config Menu's API (if it's installed)
        var configMenu = challengerMod.Helper.ModRegistry.GetApi<IGenericModConfigMenuApi>("spacechase0.GenericModConfigMenu");
        if (configMenu is null)
            return;

        // register mod
        configMenu.Register(
            mod: challengerMod.ModManifest,
            reset: () => challengerMod.Config = new ChallengerConfig(),
            save: () => challengerMod.Helper.WriteConfig(challengerMod.Config)
        );

        // add some config options
        configMenu.AddKeybind(
            mod: challengerMod.ModManifest,
            name: () => challengerMod.Helper.Translation.Get("ChallengerConfig.ButtonOpenMenu.Name"),
            tooltip: () => challengerMod.Helper.Translation.Get("ChallengerConfig.ButtonOpenMenu.Description"),
            getValue: () => challengerMod.Config.ButtonOpenMenu,
            setValue: value => challengerMod.Config.ButtonOpenMenu = value
        );
    }
}