using System.Collections;
using System.Collections.Generic;
using SOEvent.Sender;
using UnityEngine;
public class Collectable : MonoBehaviour
{
    [SerializeField]GameEvent pickedUpCollectableEvent;
    public void OnCollect()
    {
        pickedUpCollectableEvent.TriggerEvent();
        Destroy(gameObject);
    }
}
