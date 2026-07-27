using Godot;
using System;
namespace Game;
// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ----
public partial class Audio2D : AudioStreamPlayer2D
{
	public Action OnFinishAction = null;
	public bool IsInPool = false;
}
