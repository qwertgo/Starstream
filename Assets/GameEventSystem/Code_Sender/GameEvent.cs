using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SOEvent.Listener;

namespace SOEvent.Sender
{
    [CreateAssetMenu(menuName = "Events/Basic",fileName = "Basic GameEvent")]
    public class GameEvent : ScriptableObject
    {
        [SerializeField] List<GameEventListener> eventListeners = new List<GameEventListener>();

        public void TriggerEvent()
        {
            for(int i = eventListeners.Count -1;i>=0;i--)
                eventListeners[i].OnEventTriggered();
        }
        public void AddListener(GameEventListener listener)
        {
            if(!eventListeners.Contains(listener))
                eventListeners.Add(listener);
        }
        public void RemoveListener(GameEventListener listener)
        {   
            if(eventListeners.Contains(listener))
                eventListeners.Remove(listener);
        }
    }   
}