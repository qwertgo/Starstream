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
    [SerializeField] private float rotationSpeed;
    public void OnCollect()
    {
        pickedUpCollectableEvent.TriggerEvent();
        Destroy(gameObject);
        onCollectEvent?.Invoke();
        VisualEffect vfx = Instantiate(collectVFX, transform.position, transform.rotation);
        vfx.transform.localScale *= 2;
    }

    private void Start()
    {
        transform.rotation *= Quaternion.Euler(transform.up * Random.Range(0f,90));
    }

    private void Update()
    {
        transform.rotation *= Quaternion.Euler(transform.up * rotationSpeed * Time.deltaTime);    
    }
}
