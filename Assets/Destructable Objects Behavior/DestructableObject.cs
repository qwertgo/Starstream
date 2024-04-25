using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructableObject : MonoBehaviour
{
    [SerializeField] ParticleSystem particleSystem;
    private void Start() 
    {
        if(particleSystem == null && TryGetComponent<ParticleSystem>(out particleSystem) == true)
            particleSystem = GetComponent<ParticleSystem>();
    }
    void DestroyOnCrash()
    {

    }
}
