using Godot;
using System;
using System.Collections.Generic;
namespace Game;
// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ----
public static partial class Pools
{
// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ----
	public static TimerCustom NewTimer(Node parent, float waitTime, Action onTimer, bool oneShot)
	{
		TimerCustom timer = timerPool.Get(parent);
		timer.IsInPool = false;
		timer.WaitTime = waitTime;
		timer.OneShot = oneShot;
		timer.OnTimerAction = oneShot ? () =>
		{
			try
			{
				onTimer?.Invoke();
			}
			finally
			{
				RemoveTimer(timer);
			}
		} : onTimer;
		timer.Timeout += timer.OnTimerAction;
		timer.Start();
		return timer;
	}
	public static void RemoveTimer(TimerCustom timer)
	{
		if (timer.IsInPool) return;
		timer.Stop();
		timer.IsInPool = true;
		timer.Timeout -= timer.OnTimerAction;
		timer.OnTimerAction = null;
		timerPool.Return(timer);
	}
	private static Pool<TimerCustom> timerPool = new();
// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ----
    public static void LoadSound(string key, string path)
    {
        soundsPool[key] = GD.Load<AudioStream>(path);
    }
	public static Audio2D PlayAudio2D(Node parent, string key, float volume = 0f, float pitch = 1f)
	{
		if (soundsPool.TryGetValue(key, out var stream) == false)
		{
			GD.PushWarning($"Sound key '{key}' not loaded!");
			return null;
		}
		Audio2D audio = audio2DPool.Get(parent);
		audio.IsInPool = false;
		audio.Stream = stream;
		audio.VolumeDb = volume;
        audio.PitchScale = pitch;
		audio.OnFinishAction = () => RemoveAudio(audio);
		audio.Finished += audio.OnFinishAction;
		audio.Play();
		return audio;
	}
	public static void RemoveAudio(Audio2D audio)
	{
		if (audio.IsInPool) return;
		audio.Stop();
		audio.IsInPool = true;
		audio.Finished -= audio.OnFinishAction;
		audio.OnFinishAction = null;
		audio2DPool.Return(audio);
	}
	private static Pool<Audio2D> audio2DPool = new();
    private static Dictionary<string, AudioStream> soundsPool = [];
// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ----
	public static LightOccluder2D NewOccluder(Node parent, Vector2[] polygons)
	{
		LightOccluder2D occluder = occluderPool.Get(parent);
		occluder.Occluder ??= new();
		occluder.Occluder.Polygon = polygons;
		return occluder;
	}
	public static void RemoveOccluder(LightOccluder2D occluder)
	{
		if (occluder.Occluder.Polygon != null)
			occluder.Occluder.Polygon = null;
		occluderPool.Return(occluder);
	}
	private static Pool<LightOccluder2D> occluderPool = new();
// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ----
// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ----
// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ----
/*	public static Creature NewCreature(string name, Node parent)
	{
		Creature creature = _creaturePool.Get(parent);
		creature.Init(name);
		return creature;
	}
	public static void RemoveCreature(Creature creature) => _creaturePool.Return(creature);
	private static readonly PackedScene _creatureScene = GD.Load<PackedScene>("res://Scenes/Creature.tscn");
	private static Pool<Creature> _creaturePool = new(() => _creatureScene.Instantiate<Creature>());
*/// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ----

}
