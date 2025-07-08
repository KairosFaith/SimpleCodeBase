using System.Collections.Generic;
public delegate void GameEvent(object sender, object[] args);
public delegate void CommonGameEvent(EventType type, object sender, object[] args);
public enum EventType
{
    RegisterTile,
}
public static class EventEngine
{
    static Dictionary<EventType, GameEvent> _Events = new Dictionary<EventType, GameEvent>();
    public static CommonGameEvent OnBroadcastEvent;
    public static void SubscribeEvent(EventType type, GameEvent toAdd)
    {
        if (_Events.ContainsKey(type))
            _Events[type] += toAdd;
        else
            _Events.Add(type, toAdd);
    }
    public static void UnsubscribeEvent(EventType type, GameEvent toRemove)
    {
        if (_Events.TryGetValue(type, out GameEvent existing))
            existing -= toRemove;
        if (existing == null)
            _Events.Remove(type);
    }
    public static void BroadcastEvent(EventType type, object sender, params object[] args)
    {
        if (_Events.TryGetValue(type, out GameEvent gameEvent))
            gameEvent(sender, args);
        OnBroadcastEvent?.Invoke(type, sender, args);
    }
}