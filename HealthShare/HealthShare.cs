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
	}

	public void Unload() =>
		Instance = null;
}
