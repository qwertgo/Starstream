using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathSection : MonoBehaviour
{
    [SerializeField] TransitionDirection transitionDirection;
    void Start()
    {
        
    }

    void Update()
    {
        
    }
    public TransitionDirection GetDirection() => transitionDirection;
}
