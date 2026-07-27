using Godot;
namespace Game;
// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ----
public partial class Occluder2D : LightOccluder2D, IPoolable
{
	public bool IsInPool { get; set; } = false;	
	public void Clear()
	{
		if (Occluder != null)
			Occluder.Polygon = null;
	}
}
