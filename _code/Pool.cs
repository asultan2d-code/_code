using System;
using System.Collections.Generic;
using Godot;
namespace Game;
// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ----
public class Pool<T> where T : class, new()
{
    public Pool(Func<T> _factory = null, int size = 0, Signal<EventContext> signal = null)
    {
        factory = _factory ?? (() => new T());
		limit = size;
		if (limit > 0 && signal != null)
			signal.Subscribe(TrimExcess);
    }
	public T Get(Node parent = null)
	{
		T obj = _pool.Count > 0 ? _pool.Dequeue() : factory();
		if (obj is Node node)
		{
			if (node.ProcessMode != Node.ProcessModeEnum.Inherit)
				node.ProcessMode = Node.ProcessModeEnum.Inherit;
			parent?.AddChild(node);
		}
		return obj;
	}
	public void Return(T obj)
	{
		if (obj is Node node)
		{
			node.GetParent()?.RemoveChild(node);
			node.ProcessMode = Node.ProcessModeEnum.Disabled;
		}
		if (obj is IHasClear has)
			has.Clear();
		_pool.Enqueue(obj);
	}
	public void TrimExcess(EventContext _)
	{
        int excess = _pool.Count - limit;
        if (excess <= 0) return;
        for (int i = 0; i < excess; i++)
        {
            T obj = _pool.Dequeue();
            if (obj is Node node)
				node.QueueFree();
        }
	}
	private Queue<T> _pool = new();
    private Func<T> factory = null;
	private int limit = 0;
}
// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ----
public interface IHasClear
{
	void Clear();
}