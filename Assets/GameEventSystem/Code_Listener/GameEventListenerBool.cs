using UnityEngine;

using SOEvent.Sender;

namespace SOEvent.Listener
{
    [System.Serializable]
    public class BoolUnityEvent : UnityEngine.Events.UnityEvent<bool> {}
    public class GameEventListenerBool : MonoBehaviour
    {
        public GameEventBool gameEvent;
        public BoolUnityEvent response;

        void OnEnable() 
        {
            gameEvent.AddListener(this);
        }
        void OnDisable()
        {
            gameEvent.RemoveListener(this);
        }
        public void OnEventTriggered(bool theBool)
        {
            response.Invoke(theBool);
        }
    }
}