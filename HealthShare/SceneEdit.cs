namespace HealthShare;

#pragma warning disable IDE0051

internal sealed class SceneEdit : MonoBehaviour {
	private static readonly SceneEdit instance = GameObjectUtil.CreateHolder<SceneEdit>();

	internal static void EditScene(Scene prev, Scene next) {
		instance.StopAllCoroutines();

		if (GameManager.instance.IsNonGameplayScene()) {
			return;
		}

		if (BossSequenceController.IsInSequence && !HealthShare.GlobalSettings.modifyPantheons) {
			return;
		}

		_ = instance.StartCoroutine(next.name);
	}


	private IEnumerator GG_God_Tamer() {
		GameObject.Find("Entry Object")
			.LocateMyFSM("Control").Fsm.GetState("Roar")
			.InsertCustomAction(() =>
				new[] { "Lancer", "Lobster" }
					.Select(s => "Entry Object/" + s)
					.Select(s => GameObject.Find(s))
					.ShareHealth(name: "God Tamer")
			, 5);

		yield break;
	}

	private IEnumerator GG_Nailmasters() {
		GameObject.Find("Brothers/Oro")
			.LocateMyFSM("nailmaster").Fsm.GetState("Reactivate")
			.AddCustomAction(() =>
				new[] { "Oro", "Mato" }
					.Select(s => "Brothers/" + s)
					.Select(s => GameObject.Find(s))
					.ShareHealth(name: "Nailmaster Brothers")
			);

		yield break;
	}

	private IEnumerator GG_Mantis_Lords() {
		GameObject.Find("Mantis Battle/Battle Sub")
			.LocateMyFSM("Start").Fsm.GetState("Init Pause")
			.AddCustomAction(() => this.SetTimeOut(0.25f, () => new[] { 1, 2 }
				.Select(s => "Mantis Battle/Battle Sub/Mantis Lord S" + s)
				.Select(s => GameObject.Find(s))
				.ShareHealth(name: "Mantis Lords")
			));

		yield break;
	}

	private IEnumerator GG_Mantis_Lords_V() {
		GameObject.Find("Mantis Battle/Battle Sub")
			.LocateMyFSM("Start").Fsm.GetState("Init Pause")
			.AddCustomAction(() => this.SetTimeOut(0.25f, () => new[] { 1, 2, 3 }
				.Select(s => "Mantis Battle/Battle Sub/Mantis Lord S" + s)
				.Select(s => GameObject.Find(s))
				.ShareHealth(name: "Sisters of Battle")
			));

		yield break;
	}

	private IEnumerator GG_Vengefly_V() {
		_ = new[] { "Giant Buzzer Col", "Giant Buzzer Col (1)" }
			.Select(name => GameObject.Find(name))
			.ShareHealth(name: "Vengefly Kings");

		yield break;
	}
}
