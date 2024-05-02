using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructionhandler : MonoBehaviour
{
    private void OnCollisionEnter(Collision other) 
    {
        if(other.gameObject.TryGetComponent<DestructableObject>(out DestructableObject destructableObject))
        {
            Vector3 collisionPoint = GetContactpoint(other.contacts);
            destructableObject.OnCrash(collisionPoint);
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
