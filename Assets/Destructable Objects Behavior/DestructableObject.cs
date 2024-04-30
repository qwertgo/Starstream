using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class DestructableObject : MonoBehaviour
{
    public enum CollisionType {Static, Moveable}
    [SerializeField]CollisionType collisionType = CollisionType.Moveable;
    Collider collider;
    Rigidbody rb;
    private void Start() 
    {
        rb = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();

    }
    public void OnCrash()
    {
        Debug.Log($"Crashed into {gameObject.name}");

        if (collider is MeshCollider)
        {
            MeshCollider meshCollider = (MeshCollider)collider;
            meshCollider.convex = true;
            meshCollider.isTrigger = true;
        }
        else
        {
            collider.isTrigger = true;
        }
        //do stuff with particles
        //do stuff with objects transform
    }
}