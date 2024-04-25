using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PathSection : MonoBehaviour
{
    [SerializeField] TransitionDirection transitionDirection;
 
    public TransitionDirection GetDirection() => transitionDirection;
}
