using static Modding.IMenuMod;

namespace HealthShare;

public sealed partial class HealthShare : IMenuMod {
	bool IMenuMod.ToggleButtonInsideMenu => true;

	List<MenuEntry> IMenuMod.GetMenuData(MenuEntry? toggleButtonEntry) => new() {
		new(
			"Modify Bosses",
			ModMenu.StateStrings,
			"",
			i => GlobalSettings.modifyBosses = Convert.ToBoolean(i),
			() => Convert.ToInt32(GlobalSettings.modifyBosses)
		),
		new(
			"Modify Pantheons",
			ModMenu.StateStrings,
			"",
			i => GlobalSettings.modifyPantheons = Convert.ToBoolean(i),
			() => Convert.ToInt32(GlobalSettings.modifyPantheons)
		)
	};

	private static class ModMenu {
		internal static string[] StateStrings => new string[] {
			Lang.Get("MOH_OFF", "MainMenu"),
			Lang.Get("MOH_ON", "MainMenu")
		};
	}
}
