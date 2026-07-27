using Godot;
using System;
namespace Game;
// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ----
public partial class TimerCustom : Timer, IPoolable
{
	public Action OnTimerAction = null;
	public bool IsInPool { get; set; } = false;
	public void Clear()
	{
		Stop();
		Timeout -= OnTimerAction;
		OnTimerAction = null;
	}
}
