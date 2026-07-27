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
				Remove(timer);
			}
		} : onTimer;
		timer.Timeout += timer.OnTimerAction;
		timer.Start();
		return timer;
	}
	public static void Remove(TimerCustom timer) => timerPool.Return(timer);
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
		audio.Stream = stream;
		audio.VolumeDb = volume;
        audio.PitchScale = pitch;
		audio.OnFinishAction = () => Remove(audio);
		audio.Finished += audio.OnFinishAction;
		audio.Play();
		return audio;
	}
	public static void Remove(Audio2D audio) => audio2DPool.Return(audio);
	private static Pool<Audio2D> audio2DPool = new();
    private static Dictionary<string, AudioStream> soundsPool = [];
// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ----
	public static Occluder2D NewOccluder(Node parent, Vector2[] polygons)
	{
		Occluder2D occluder = occluderPool.Get(parent);
		occluder.Occluder ??= new();
		occluder.Occluder.Polygon = polygons;
		return occluder;
	}
	public static void Remove(Occluder2D occluder) => occluderPool.Return(occluder);
	private static Pool<Occluder2D> occluderPool = new();
}
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

