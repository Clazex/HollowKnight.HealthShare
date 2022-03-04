namespace HealthShare;

public sealed partial class HealthShare : IGlobalSettings<GlobalSettings> {
	public static GlobalSettings GlobalSettings { get; private set; } = new();
	public void OnLoadGlobal(GlobalSettings s) => GlobalSettings = s;
	public GlobalSettings OnSaveGlobal() => GlobalSettings;
}

public sealed class GlobalSettings {
	public bool modifyBosses = false;
	public bool modifyPantheons = false;
}
