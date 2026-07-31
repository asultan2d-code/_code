using System;
using System.Collections.Generic;
using Godot;
namespace Game;
using static Utilities;
// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ----
public static partial class Pools
{
// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ----
	public static OneShotTimer NewTimer(Node parent, float waitTime, Action onTimer)
	{
		OneShotTimer timer = timerPool.Get(parent);
		timer.WaitTime = waitTime > 0 ? waitTime : 1f;
		timer.SetTimerAction(onTimer, () => timerPool.Return(timer));
		timer.Start();
		return timer;
	}
	public static void Remove(OneShotTimer timer) => timerPool.Return(timer);
	private static Pool<OneShotTimer> timerPool = new();
// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ----
	public static void PlayAudio2D(Node parent, string key, float volume = 0f, float pitch = 1f, int playCount = 1,
		Signal<EventContext> shutdownSignal = null, float shutdownTimer = 0f, float fadeTime = 3f)
	{
		if (soundsPool.TryGetValue(key, out var stream) == false)
		{
			GD.PushWarning($"Sound '{key}' not found!");
			return;
		}
		Audio2D audio = audio2DPool.Get(parent);
		audio.SetReturnAction(() => audio2DPool.Return(audio));
		if (StreamLooped(stream))
		{
			if (shutdownSignal != null)
				shutdownSignal.SubscribeOnce((_) => audio.StartFade(fadeTime));
			else if (shutdownTimer > 0)
				NewTimer(audio, shutdownTimer, () => audio.StartFade(fadeTime));
			else
			{
				GD.PushWarning($"Sound '{key}' is loopes and no signal/timer to stop!");
				audio2DPool.Return(audio);
				return;
			}
		}
		audio.PlayCount = Clamp(playCount, 1, 100);
		audio.Stream = stream;
		audio.VolumeDb = Clamp(volume, -80f, 24f);
        audio.PitchScale = Clamp(pitch, 0.1f, 4f);
		audio.Play();
	}
	public static void LoadSound(string key, string path, LoopTypeEnum loopType = LoopTypeEnum.Not)
	{
		AudioStream audio = GD.Load<AudioStream>(path);
		if (ValidAudioStream(audio) == false)
		{
			GD.PushWarning($"Audio ('{path}') is not mp3, ogg or wav!");
			return;
		}
		switch (loopType)
		{
			case LoopTypeEnum.Not:
				soundsPool[key] = audio;
				break;
			case LoopTypeEnum.Copy:
				soundsPool[key] = audio;
				AudioStream loopedAudio = (AudioStream)audio.Duplicate();
				SetLoop(loopedAudio);
				soundsPool[key + "_loop"] = loopedAudio;
				break;
			case LoopTypeEnum.Only:
				SetLoop(audio);
				soundsPool[key + "_loop"] = audio;
				break;
		}
	}
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
public enum LoopTypeEnum { Not, Copy, Only }
