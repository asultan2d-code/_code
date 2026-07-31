using Godot;
using System;
namespace Game;
// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ----
public partial class Audio2D : AudioStreamPlayer2D, IPoolable
{
	public Audio2D()
	{
		removeAction = () => Pools.Remove(this);
	}
	private Action removeAction;
	public bool IsInPool { get; set; } = false;

	public void Clear()
	{
		Stop();
		if (actionSubscribed == false) return;
		actionSubscribed = false;
		Finished -= removeAction;
	}
	private bool actionSubscribed = false;
	public void RemoveOnFinished()
	{
		if (actionSubscribed) return;
		actionSubscribed = true;
		Finished += removeAction;
	}
	public void StartFade(float fadeTime)
	{
		if (fadeTime <= 0) return;
		Tween tween = CreateTween();
		tween.TweenProperty(this, "volume_db", -60f, fadeTime);
		tween.Finished += removeAction;
	}
}
