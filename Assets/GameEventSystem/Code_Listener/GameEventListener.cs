using UnityEngine;
using UnityEngine.Events;

using SOEvent.Sender;

namespace SOEvent.Listener
{
    public class GameEventListener : MonoBehaviour
    {
        public GameEvent gameEvent;
        public UnityEvent response;

        void OnEnable() 
        {
            gameEvent.AddListener(this);
        }
        void OnDisable()
        {
            gameEvent.RemoveListener(this);
        }
        public void OnEventTriggered()
        {
            response.Invoke();
        }
    }
}