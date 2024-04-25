using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshCollider))]
public class DestructableObject : MonoBehaviour
{
    [SerializeField] ParticleSystem particleSystem;
    MeshCollider meshCollider;
    private void Start() 
    {
        if(particleSystem == null && TryGetComponent<ParticleSystem>(out particleSystem) == true)
            particleSystem = GetComponent<ParticleSystem>();
        meshCollider = GetComponent<MeshCollider>();
    }
    public void OnCrash()
    {
        Debug.Log($"Crashed into {gameObject.name}");
        meshCollider.isTrigger = true;
        //do stuff with particles
        //do stuff with objects transform
    }
}