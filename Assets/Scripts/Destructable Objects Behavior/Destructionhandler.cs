using System.Collections;
using System.Collections.Generic;
using SOEvent.Sender;
using Unity.VisualScripting;
using UnityEngine;

public class Destructionhandler : MonoBehaviour
{
    [SerializeField]GameEvent onCrashDestructable;
    private void OnCollisionEnter(Collision other) 
    {
        if(other.gameObject.TryGetComponent<DestructableObject>(out DestructableObject destructableObject))
        {
            Vector3 collisionPoint = GetContactpoint(other.contacts);
            destructableObject.OnCrash(collisionPoint);
            onCrashDestructable.TriggerEvent();
        }
    }
    private void OnTriggerEnter(Collider other) 
    {
        if(other.gameObject.TryGetComponent<Collectable>(out Collectable collectable))
        {
            collectable.OnCollect();
        }
    }

    Vector3 GetContactpoint(ContactPoint[] contacts)
    {
        return contacts[0].point;
    }
}
