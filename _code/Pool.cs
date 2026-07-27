using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ----
public class Pool<T> where T : class, new()
{
    public Pool(Func<T> factory = null, int size = 0, Signal signal = null)
    {
        _factory = factory ?? (() => new T()); _maxSize = size;
		if (_maxSize > 0 && signal != null) signal.Subscribe(TrimExcess);
    }
	public T Get(Node parent = null)
	{
		T obj = _pool.Count > 0 ? _pool.Dequeue() : _factory();
		if (obj is Node node)
		{
			if (node.ProcessMode != Node.ProcessModeEnum.Inherit) node.ProcessMode = Node.ProcessModeEnum.Inherit;
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
		if (obj is IHasClear has) has.Clear();
		_pool.Enqueue(obj);
	}
	public void TrimExcess()
	{
        int excess = _pool.Count - _maxSize;
        if (excess <= 0) return;
        for (int i = 0; i < excess; i++)
        {
            T obj = _pool.Dequeue();
            if (obj is Node node) node.QueueFree();
        }
	}
	private Queue<T> _pool = new();
    private Func<T> _factory = null;
	private int _maxSize = 0;
}
// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ----
public interface IHasClear
{
	void Clear();
}
public class Signal
{
    private List<Action> _handlers = [];
    public void Subscribe(Action handler) => _handlers.Add(handler);
    public void SubscribeOnce(Action handler)
    {
        void wrapper()
        {
            handler();
            Unsubscribe(wrapper);
        }
        Subscribe(wrapper);
    }
    public void Unsubscribe(Action handler) => _handlers.Remove(handler);
    public void Invoke()
    {
        for (int i = 0; i < _handlers.Count; i++) _handlers[i]?.Invoke();
    }
    public void UnsubscribeAll() => _handlers.Clear();
    public int GetSubsCount() => _handlers.Count();
}
