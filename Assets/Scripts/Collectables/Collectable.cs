using System.Collections;
using System.Collections.Generic;
using SOEvent.Sender;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.VFX;
public class Collectable : MonoBehaviour
{
    [SerializeField]GameEvent pickedUpCollectableEvent;
    [SerializeField]UnityEvent onCollectEvent;
    [SerializeField]VisualEffect collectVFX;
    public void OnCollect()
    {
        pickedUpCollectableEvent.TriggerEvent();
        Destroy(gameObject);
        onCollectEvent?.Invoke();
        VisualEffect vfx = Instantiate(collectVFX, transform.position, transform.rotation);
        vfx.transform.localScale *= 2;
    }
}
