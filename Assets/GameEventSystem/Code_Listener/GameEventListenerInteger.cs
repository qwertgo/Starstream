using UnityEngine;

using SOEvent.Sender;

namespace SOEvent.Listener
{
    [System.Serializable]
    public class IntegerUnityEvent : UnityEngine.Events.UnityEvent<int> {}
    public class GameEventListenerInteger : MonoBehaviour
    {
        public GameEventInteger gameEvent;
        public IntegerUnityEvent response;

        void OnEnable() 
        {
            gameEvent.AddListener(this);
        }
        void OnDisable()
        {
            gameEvent.RemoveListener(this);
        }
        public void OnEventTriggered(int integer)
        {
            response.Invoke(integer);
        }
    }
}