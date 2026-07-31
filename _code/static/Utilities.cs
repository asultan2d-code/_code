using System;
using System.Collections.Generic;
using Godot;
namespace Game;
// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ----
public static partial class Utilities
{
	private static readonly Random rnd = new();
// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ----
    public static int RandI(int max) =>
        rnd.Next(max);
	public static int RandI(int min, int max) =>
		rnd.Next(min, max);
	public static float RandF() =>
		(float)rnd.NextDouble();
	public static float RandF(float max) =>
		(float)rnd.NextDouble() * max;
	public static float RandF(float min, float max) =>
		min + (float)rnd.NextDouble() * (max - min);
	public static bool RandomBool() =>
		RandI(2) == 0;
    public static bool Chance(int percent) =>
        RandI(100) < percent;
// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ----
	public static T RandomItem<T>(IList<T> list) =>
		list[RandI(list.Count)];
// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ----
	public static int Clamp(int value, int min, int max) =>
		Math.Clamp(value, min, max);
	public static int Clamp(int value, int max) =>
		Math.Clamp(value, 0, max);
	public static float Clamp(float value, float min, float max) =>
		Math.Clamp(value, min, max);
	public static float Clamp(float value, float max) =>
		Math.Clamp(value, 0, max);
// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ----
	public static bool ValidAudioStream(AudioStream stream) => stream is AudioStreamMP3 or AudioStreamOggVorbis or AudioStreamWav;
	public static bool StreamLooped(AudioStream stream)
	{
		if (stream is AudioStreamMP3 mp3) return mp3.Loop;
		if (stream is AudioStreamOggVorbis ogg) return ogg.Loop;
		if (stream is AudioStreamWav wav) return wav.LoopMode == AudioStreamWav.LoopModeEnum.Forward;
		return false;
	}
	public static void SetLoop(AudioStream stream, bool loop = true)
	{
		if (stream is AudioStreamMP3 mp3) mp3.Loop = loop;
		else if (stream is AudioStreamOggVorbis ogg) ogg.Loop = loop;
		else if (stream is AudioStreamWav wav) wav.LoopMode = loop ? AudioStreamWav.LoopModeEnum.Forward : AudioStreamWav.LoopModeEnum.Disabled;
	}
}
