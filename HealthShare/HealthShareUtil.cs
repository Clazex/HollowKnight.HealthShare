using MonoMod.ModInterop;
using MonoMod.Utils;

namespace HealthShare;

[PublicAPI]
[ModExportName(nameof(HealthShare))]
public static class HealthShareUtil {
	public static SharedHealthManager? GetSharedHM(this HealthManager self) =>
		new DynamicData(self).Get<SharedHealthManager>("sharedHM");

	public static bool TryGetSharedHM(this HealthManager self, [NotNullWhen(true)] out SharedHealthManager? shm) =>
		(shm = GetSharedHM(self)) != null;

	public static bool IsSharing(this HealthManager self) => self.TryGetSharedHM(out _);


	public static SharedHealthManager? GetBackedSharedHM(this HealthManager self) =>
		new DynamicData(self).Get<SharedHealthManager>("backedSharedHM");

	public static bool TryGetBackedSharedHM(this HealthManager self, [NotNullWhen(true)] out SharedHealthManager? shm) =>
		(shm = GetBackedSharedHM(self)) != null;

	public static bool IsBackingHM(this HealthManager self) => self.TryGetBackedSharedHM(out _);


	public static void AddToShared(this HealthManager self, SharedHealthManager shm) =>
		shm.Add(self);

	public static void AddToShared(this IEnumerable<HealthManager> self, SharedHealthManager shm) =>
		shm.Add(self);

	public static void AddToShared(this GameObject self, SharedHealthManager shm) {
		if (self.GetComponent<HealthManager>() is HealthManager hm) {
			shm.Add(hm);
		}
	}

	public static void AddToShared(this IEnumerable<GameObject> self, SharedHealthManager shm) => self
		.Select(go => go.GetComponent<HealthManager>())
		.Where(hm => hm != null)
		.AddToShared(shm);

	public static void ShareHealth(this IEnumerable<HealthManager> self, int initialHP = 0, string name = "SharedHealthManager") =>
		self.AddToShared(SharedHealthManager.Create(name, initialHP));

	public static void ShareHealth(this IEnumerable<GameObject> self, int initialHP = 0, string name = "SharedHealthManager") =>
		self.AddToShared(SharedHealthManager.Create(name, initialHP));

	public static void ShareWith(this HealthManager self, params HealthManager[] hms) =>
		SharedHealthManager.Create(self.name + " Shared Health", hms.Prepend(self));

	public static void StopSharing(this HealthManager self, int healthTaking) {
		if (!self.TryGetSharedHM(out SharedHealthManager? shm)) {
			throw new InvalidOperationException("Specified HM is not sharing health");
		}

		shm.Remove(self, healthTaking);
	}
}
