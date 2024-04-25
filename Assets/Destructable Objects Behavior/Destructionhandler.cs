using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructionhandler : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) 
    {
        if(other.gameObject.TryGetComponent<DestructableObject>(out DestructableObject destructableObject))
        {
            destructableObject.OnCrash();
        }
    }
}
