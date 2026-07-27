using Godot;
using System;
namespace Game;
// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ----
public partial class Audio2D : AudioStreamPlayer2D, IPoolable
{
	public Action OnFinishAction = null;
	public bool IsInPool { get; set; } = false;
	public void Clear()
	{
		Stop();
		Finished -= OnFinishAction;
		OnFinishAction = null;
	}
}
