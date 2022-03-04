namespace HealthShare;

internal static class Utils {
	internal static void ForEach<T>(this IEnumerable<T> self, Action<T> action) {
		foreach (T t in self) {
			action.Invoke(t);
		}
	}

	internal static void InsertAction(this FsmState state, int index, FsmStateAction action) {
		FsmStateAction[] oldActions = state.Actions;
		var actions = new FsmStateAction[oldActions.Length + 1];
		int i = 0;

		for (; i < index; i++) {
			actions[i] = oldActions[i];
		}

		actions[i] = action;

		for (; i < oldActions.Length; i++) {
			actions[i + 1] = oldActions[i];
		}

		state.Actions = actions;
	}

	internal static void AddAction(this FsmState self, FsmStateAction action) =>
		self.InsertAction(self.Actions.Length, action);

	internal static void SetTimeOut(float timeOut, Action self) =>
		GameManager.instance.sm.StartCoroutine(TimeOutCoroutine(self, timeOut));

	private static IEnumerator TimeOutCoroutine(Action action, float timeOut) {
		yield return new WaitForSeconds(timeOut);
		action.Invoke();
	}
}

internal sealed class InvokeAction : FsmStateAction {
	public Action? action;

	public InvokeAction(Action action) =>
		this.action = action;

	public override void Reset() {
		action = null;
		base.Reset();
	}

	public override void OnEnter() {
		action?.Invoke();
		Finish();
	}
}
