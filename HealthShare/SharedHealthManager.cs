using GlobalEnums;

using MonoMod.Utils;

namespace HealthShare;

[PublicAPI]
public class SharedHealthManager : MonoBehaviour {
	public HealthManager BackingHM { get; private init; }

	private readonly List<HealthManager> HMs = new();

	public IEnumerable<HealthManager> Members => HMs;

	public int HP {
		get => BackingHM.hp;
		set => BackingHM.hp = value;
	}

	#region Create

	public static SharedHealthManager Create(string name = "SharedHealthManager", int initialHP = 0) {
		GameObject go = new(name, typeof(SharedHealthManager));
		SharedHealthManager shm = go.GetComponent<SharedHealthManager>();
		shm.HP += initialHP;
		return shm;
	}

	public static SharedHealthManager Create(string name, int initialHP, IEnumerable<HealthManager> hms) {
		SharedHealthManager shm = Create(name, initialHP);
		shm.Add(hms);
		return shm;
	}

	public static SharedHealthManager Create(string name, IEnumerable<HealthManager> hms) =>
		Create(name, 0, hms);

	public static SharedHealthManager Create(string name, params HealthManager[] hms) =>
		Create(name, hms.AsEnumerable());

	#endregion

	private SharedHealthManager() {
		BackingHM = gameObject.AddComponent<HealthManager>();
		BackingHM.OnDeath += KillMembers;
		new DynamicData(BackingHM).Set("backedSharedHM", this);
		gameObject.MarkAsBoss();

		// Tricks Debug Enemies Panel
		gameObject.layer = (int) PhysLayers.ENEMIES;
		gameObject.AddComponent<NonBouncer>();
		gameObject.AddComponent<BoxCollider2D>().size =
			new(short.MaxValue, short.MaxValue);
	}

	public void OnDestroy() =>
		Die();

	#region Adding HM

	public void Add(HealthManager hm) {
		if (hm == BackingHM) {
			throw new InvalidOperationException("Cannot add self backing hm to shared hm");
		}

		DynamicData dyn = new(hm);
		if (dyn.Get<SharedHealthManager>("sharedHM") != null) {
			throw new InvalidOperationException("HM already add to another shared HM");
		}

		HMs.Add(hm);
		hm.OnDeath += CheckDie;

		HP += hm.hp;
		hm.hp = int.MaxValue;

		dyn.Set("sharedHM", this);

		RefreshEnemyHPBar(true);
	}

	public void Add(IEnumerable<HealthManager> hms) {
		foreach (HealthManager hm in hms) {
			Add(hm);
		}
	}

	public void Add(params HealthManager[] hms) => Add(hms.AsEnumerable());

	#endregion

	public void Remove(HealthManager hm, int healthTaking) {
		if (healthTaking < 0) {
			throw new ArgumentOutOfRangeException(nameof(healthTaking));
		}

		if (hm.GetSharedHM() != this) {
			throw new InvalidOperationException("Specified HM is not sharing in current Shared HM");
		}

		HMs.Remove(hm);
		hm.hp = healthTaking;
		HP -= healthTaking;

		DynamicData dyn = new(hm);
		dyn.Set("sharedHM", null);

		hm.gameObject.EnableHPBar();
	}

	public void CheckDie() {
		if (HMs.TrueForAll(hm => hm.isDead)) {
			Die();
		}
	}

	public void Die() =>
		BackingHM.Die(null, AttackTypes.RuinsWater, true);

	public void KillMembers() {
		foreach (HealthManager hm in HMs) {
			try {
				hm.Die(null, AttackTypes.Generic, true);
			} catch (NullReferenceException) {
			}
		}
	}

	public void RefreshEnemyHPBar(bool recheckMembers = false) {
		if (recheckMembers) {
			HMs.ForEach(hm => hm.gameObject.DisableHPBar());
		}

		gameObject.RefreshHPBar();
	}

	#region Static Helpers

	static SharedHealthManager() {
		On.HealthManager.TakeDamage += ReportDamage;
		On.HealthManager.ApplyExtraDamage += ReportExtraDamage;
	}

	private static void ReportDamage(On.HealthManager.orig_TakeDamage orig, HealthManager self, HitInstance hit) {
		orig(self, hit);

		if (self.TryGetSharedHM(out SharedHealthManager? shm)) {
			shm.BackingHM.ApplyExtraDamage(hit.DamageDealt);
		}
	}

	private static void ReportExtraDamage(On.HealthManager.orig_ApplyExtraDamage orig, HealthManager self, int damage) {
		orig(self, damage);

		if (self.TryGetSharedHM(out SharedHealthManager? shm)) {
			shm.BackingHM.ApplyExtraDamage(damage);
		}
	}

	#endregion
}
