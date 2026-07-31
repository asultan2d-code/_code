using Godot;
using System;
namespace Game;
// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ----
public partial class Audio2D : AudioStreamPlayer2D, IPoolable
{
	public bool IsInPool { get; set; } = false;
	public int PlayCount = 1;
	private Action returnAction = null;
	public void SetReturnAction(Action action)
	{
		if (returnAction != null) return;
		returnAction = action;
		Finished += OnFinished;
	}
	public void Clear()
	{
		Stop();
	}
	public void StartFade(float fadeTime)
	{
		if (fadeTime <= 0) return;
		Tween tween = CreateTween();
		tween.TweenProperty(this, "volume_db", -60f, fadeTime);
		tween.Finished += returnAction;
	}
	private void OnFinished()
	{
		PlayCount--;
		if (PlayCount <= 0)
			returnAction();
		else
			Play();
	}
}
