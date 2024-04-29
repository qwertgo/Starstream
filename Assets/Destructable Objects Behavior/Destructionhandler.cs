using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructionhandler : MonoBehaviour
{
    private void OnCollisionEnter(Collision other) 
    {
        if(other.gameObject.TryGetComponent<DestructableObject>(out DestructableObject destructableObject))
        {
            destructableObject.OnCrash();
        }
        if(other.gameObject.TryGetComponent<Collectable>(out Collectable collectable))
        {
            collectable.OnCollect();
        }
    }
}
