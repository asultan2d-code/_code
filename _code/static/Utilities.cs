using System;
using System.Collections.Generic;
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
	public static T RandomItem<T>(IList<T> list) =>
		list[RandI(list.Count)];
    public static bool Chance(int percent) =>
        RandI(100) < percent;
	public static int Clamp(int value, int min, int max) =>
		Math.Clamp(value, min, max);
	public static int Clamp(int value, int max) =>
		Math.Clamp(value, 0, max);
	public static float Clamp(float value, float min, float max) =>
		Math.Clamp(value, min, max);
	public static float Clamp(float value, float max) =>
		Math.Clamp(value, 0, max);
}
//    public static async Task Wait(int delay = 100) =>
//        await Task.Delay(delay);
//    public static int CountDoublings(int exp) =>
//        (int)Math.Round(Math.Log2((double)exp / FIRST_LEVEL_EXP));
//    public static Texture2D LoadItemTexture(String name, bool icon) =>
//        ResourceLoader.Load<Texture2D>("res://Textures/Items/" + name + (icon == true ? "_i.png" : ".png"));



// Для 2D – угол поворота от from к to (в радианах)
/*public static float Angle(Vector2 from, Vector2 to) =>
    MathF.Atan2(to.y - from.y, to.x - from.x);

// Плавное движение числа к цели
public static float MoveTowards(float current, float target, float maxDelta) =>
    Math.Abs(target - current) <= maxDelta ? target : current + Math.Sign(target - current) * maxDelta;

public static string FormatTime(float seconds, bool showHours = false)
{
    var ts = TimeSpan.FromSeconds(seconds);
    return showHours ? ts.ToString(@"hh\:mm\:ss") : ts.ToString(@"mm\:ss");
}

public static void Swap<T>(ref T a, ref T b) =>
    (a, b) = (b, a);

public static bool Approximately(float a, float b, float epsilon = 1e-6f) =>
    Math.Abs(a - b) < epsilon;

public static float Lerp(float a, float b, float t) => a + (b - a) * t;
public static float InverseLerp(float a, float b, float value) => (value - a) / (b - a);
public static float Remap(float value, float fromMin, float fromMax, float toMin, float toMax) =>
    Lerp(toMin, toMax, InverseLerp(fromMin, fromMax, value));
public static float Clamp01(float value) => Clamp(value, 0f, 1f);

public static bool IsIndexValid<T>(IList<T> list, int index) =>
    index >= 0 && index < list.Count;
public static bool IsNullOrEmpty<T>(IList<T> list) =>
    list == null || list.Count == 0;
*/