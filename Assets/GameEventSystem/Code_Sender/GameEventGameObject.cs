using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SOEvent.Listener;

namespace SOEvent.Sender
{
    [CreateAssetMenu(menuName = "Events/GameObject",fileName = "GameObject GameEvent")]
    public class GameEventGameObject : ScriptableObject
    {
        [SerializeField] List<GameEventListenerGameObject> eventListeners = new List<GameEventListenerGameObject>();

        public void TriggerEvent(GameObject theGameObject)
        {
            for(int i = eventListeners.Count -1;i>=0;i--)
                eventListeners[i].OnEventTriggered(theGameObject);
        }
        public void AddListener(GameEventListenerGameObject listener)
        {
            if(!eventListeners.Contains(listener))
                eventListeners.Add(listener);
        }
        public void RemoveListener(GameEventListenerGameObject listener)
        {   
            if(eventListeners.Contains(listener))
                eventListeners.Remove(listener);
        }
    }  
}