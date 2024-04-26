using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshCollider))]
public class DestructableObject : MonoBehaviour
{
    public enum CollisionType {Static, Moveable}
    [SerializeField]CollisionType collisionType = CollisionType.Moveable;
    MeshCollider meshCollider;
    Rigidbody rb;
    private void Start() 
    {
        rb = GetComponent<Rigidbody>();
        meshCollider = GetComponent<MeshCollider>();

    }
    public void OnCrash()
    {
        Debug.Log($"Crashed into {gameObject.name}");
        meshCollider.convex = true;
        meshCollider.isTrigger = true;
        //do stuff with particles
        //do stuff with objects transform
    }
}