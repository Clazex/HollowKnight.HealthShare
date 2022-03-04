using MonoMod.ModInterop;

namespace HealthShare;

internal static class EnemyHPBar {
#pragma warning disable CS0649
	[ModImportName(nameof(EnemyHPBar))]
	private static class EnemyHPBarImport {
		public static Action<GameObject>? DisableHPBar;
		public static Action<GameObject>? EnableHPBar;
		public static Action<GameObject>? RefreshHPBar;
		public static Action<GameObject>? MarkAsBoss;
	}
#pragma warning restore CS0649

	static EnemyHPBar() =>
		typeof(EnemyHPBarImport).ModInterop();

	internal static void DisableHPBar(this GameObject self) =>
		EnemyHPBarImport.DisableHPBar?.Invoke(self);

	internal static void EnableHPBar(this GameObject self) =>
		EnemyHPBarImport.EnableHPBar?.Invoke(self);

	internal static void RefreshHPBar(this GameObject self) =>
		EnemyHPBarImport.RefreshHPBar?.Invoke(self);

	internal static void MarkAsBoss(this GameObject self) =>
		EnemyHPBarImport.MarkAsBoss?.Invoke(self);
}
