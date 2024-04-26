using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshCollider))]
public class DestructableObject : MonoBehaviour
{
    public enum CollisionType {Static, Moveable}
    [SerializeField]CollisionType collisionType = CollisionType.Moveable;
    [SerializeField]ParticleSystem particleSystem;
    MeshCollider meshCollider;
    Rigidbody rb;
    private void Start() 
    {
        rb = GetComponent<Rigidbody>();

        if(particleSystem == null && TryGetComponent<ParticleSystem>(out particleSystem) == true)
            particleSystem = GetComponent<ParticleSystem>();
        meshCollider = GetComponent<MeshCollider>();

        if(collisionType == CollisionType.Static)
            rb.isKinematic = true;
    }
    public void OnCrash()
    {
        Debug.Log($"Crashed into {gameObject.name}");
        meshCollider.isTrigger = true;
        //do stuff with particles
        //do stuff with objects transform
    }
}