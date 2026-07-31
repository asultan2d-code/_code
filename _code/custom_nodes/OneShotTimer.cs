using Godot;
using System;
namespace Game;
// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ----
public partial class OneShotTimer : Timer, IPoolable
{
	public OneShotTimer()
	{
		OneShot = true;
	}
	public bool IsInPool { get; set; } = false;
	private Action onTimerAction;
	private bool actionSubscribed = false;
	public void SetTimerAction(Action action, Action returnAction)
	{
		if (actionSubscribed) return;
		actionSubscribed = true;
		onTimerAction = action != null ? () => { try { action(); } finally { returnAction(); } } : () => returnAction();
		Timeout += onTimerAction;
	}
	public void Clear()
	{
		Stop();
		Timeout -= onTimerAction;
		onTimerAction = null;
		actionSubscribed = false;
	}
}
