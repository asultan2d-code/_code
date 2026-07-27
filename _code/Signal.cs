using System;
using System.Collections.Generic;
namespace Game;
// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ----
public class Signal<EventContext>
{
    private List<Action<EventContext>> handlers = [];
    public void Subscribe(Action<EventContext> handler) => handlers.Add(handler);
    public void SubscribeOnce(Action<EventContext> handler)
    {
		void wrapper(EventContext arg1)
		{
			handler(arg1);
			Unsubscribe(wrapper);
		}
		Subscribe(wrapper);
    }
    public void Unsubscribe(Action<EventContext> handler) => handlers.Remove(handler);
    public void Invoke(EventContext arg1)
    {
        for (int i = 0; i < handlers.Count; i++) handlers[i]?.Invoke(arg1);
    }
    public void UnsubscribeAll() => handlers.Clear();
    public int GetSubsCount() => handlers.Count;
}
// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ----
public class EventContext
{
	public EventContext()
	{
	}
}
