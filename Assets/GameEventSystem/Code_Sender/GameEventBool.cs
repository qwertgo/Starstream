using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SOEvent.Listener;

namespace SOEvent.Sender
{
    [CreateAssetMenu(menuName = "Events/Bool",fileName = "Bool GameEvent")]
    public class GameEventBool : ScriptableObject
    {
        [SerializeField] List<GameEventListenerBool> eventListeners = new List<GameEventListenerBool>();

        public void TriggerEvent(bool theBool)
        {
            for(int i = eventListeners.Count -1;i>=0;i--)
                eventListeners[i].OnEventTriggered(theBool);
        }
        public void AddListener(GameEventListenerBool listener)
        {
            if(!eventListeners.Contains(listener))
                eventListeners.Add(listener);
        }
        public void RemoveListener(GameEventListenerBool listener)
        {   
            if(eventListeners.Contains(listener))
                eventListeners.Remove(listener);
        }
    }
}