using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SOEvent.Listener;

namespace SOEvent.Sender
{
    [CreateAssetMenu(menuName = "Events/Integer",fileName = "Integer GameEvent",order = 1)]
    public class GameEventInteger : ScriptableObject
    {
        [SerializeField] List<GameEventListenerInteger> eventListeners = new List<GameEventListenerInteger>();

        public void TriggerEvent(int integer)
        {
            for(int i = eventListeners.Count -1;i>=0;i--)
                eventListeners[i].OnEventTriggered(integer);
        }
        public void AddListener(GameEventListenerInteger listener)
        {
            if(!eventListeners.Contains(listener))
                eventListeners.Add(listener);
        }
        public void RemoveListener(GameEventListenerInteger listener)
        {   
            if(eventListeners.Contains(listener))
                eventListeners.Remove(listener);
        }
    }   
}