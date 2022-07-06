namespace HealthShare;

public sealed partial class HealthShare : Mod {
	public static HealthShare? Instance { get; private set; }
	public static HealthShare UnsafeInstance => Instance!;

	internal static readonly Lazy<string> Version = AssemblyUtil
#if DEBUG
		.GetMyDefaultVersionWithHash();
#else
		.GetMyDefaultVersion();
#endif

	public override string GetVersion() => Version.Value;

	public override void Initialize() {
		if (Instance != null) {
			LogWarn("Attempting to initialize multiple times, operation rejected");
			return;
		}

		Instance = this;

		OsmiHooks.SceneChangeHook += SceneEdit.EditScene;
	}
}
