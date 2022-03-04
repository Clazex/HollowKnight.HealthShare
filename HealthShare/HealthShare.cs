using MonoMod.ModInterop;

namespace HealthShare;

public sealed partial class HealthShare : Mod, ITogglableMod {
	public static HealthShare? Instance { get; private set; }
	public static HealthShare UnsafeInstance => Instance!;

	internal static readonly Lazy<string> Version = new(() => Assembly
		.GetExecutingAssembly()
		.GetCustomAttribute<AssemblyInformationalVersionAttribute>()
		.InformationalVersion
#if DEBUG
		+ "-dev"
#endif
	);

	public override string GetVersion() => Version.Value;

	public HealthShare() =>
		typeof(HealthShareUtil).ModInterop();

	public override void Initialize() {
		if (Instance != null) {
			LogWarn("Attempting to initialize multiple times, operation rejected");
			return;
		}

		Instance = this;

		USceneManager.activeSceneChanged += EditScene;
	}

	public void Unload() {
		USceneManager.activeSceneChanged -= EditScene;

		Instance = null;
	}

	internal static void EditScene(Scene _, Scene next) {
		if (GameManager.instance.IsNonGameplayScene()) {
			return;
		}

		if (!GlobalSettings.modifyBosses) {
			return;
		}

		if (BossSequenceController.IsInSequence && !GlobalSettings.modifyPantheons) {
			return;
		}

		SceneEdit.TryEdit(next.name);
	}
}
