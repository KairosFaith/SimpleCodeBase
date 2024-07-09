using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public delegate void GameEvent(object sender, params object[] args);
public static class EventEngine 
{
    static Dictionary<EventKey, GameEvent> _Events = new Dictionary<EventKey, GameEvent>();
    public static void SubscribeEvent(EventKey key, GameEvent toAdd)
    {
        if (_Events.TryGetValue(key, out GameEvent existing))
            existing += toAdd;
        else
            _Events.Add(key, toAdd);
    }
    public static void UnsubscribeEvent(EventKey key, GameEvent toRemove)
    {
        if (_Events.TryGetValue(key, out GameEvent existing))
            existing -= toRemove;
        if (existing == null)
            _Events.Remove(key);
    }
    public static void BroadcastEvent(EventKey key, object sender, params object[] args)
    {
        if (_Events.TryGetValue(key, out GameEvent gameEvent))
            gameEvent?.Invoke(sender, args);
    }
}
public enum EventKey
{

}