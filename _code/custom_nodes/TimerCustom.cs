using Godot;
using System;
namespace Game;
// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ----
public partial class TimerCustom : Timer, IPoolable
{
	public TimerCustom()
	{
		OneShot = true;
	}
	public bool IsInPool { get; set; } = false;
	private Action onTimerAction;
	private bool actionSubscribed = false;
	public void SetTimerAction(Action action)
	{
		if (actionSubscribed) return;
		actionSubscribed = true;
		onTimerAction = action != null ? () => { try { action(); } finally { Pools.Remove(this); } } : () => Pools.Remove(this);
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
