using UnityEngine;

using SOEvent.Sender;

namespace SOEvent.Listener
{
    [System.Serializable]
    public class GameObjectUnityEvent : UnityEngine.Events.UnityEvent<GameObject> {}
    public class GameEventListenerGameObject : MonoBehaviour
    {
        public GameEventGameObject gameEvent;
        public GameObjectUnityEvent response;

        void OnEnable() 
        {
            gameEvent.AddListener(this);
        }
        void OnDisable()
        {
            gameEvent.RemoveListener(this);
        }
        public void OnEventTriggered(GameObject theGameObject)
        {
            response.Invoke(theGameObject);
        }
    }
}