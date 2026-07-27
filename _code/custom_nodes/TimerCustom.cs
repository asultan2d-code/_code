using Godot;
using System;
namespace Game;
// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ----
public partial class TimerCustom : Timer
{
	public Action OnTimerAction = null;
	public bool IsInPool = false;
}
