using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Assets/Difficulty")]
public class Difficulty : ScriptableObject
{
    public float forwardAcceleration;
    public float startSpeed;
    public float startMaxSpeed;
    public float timeForStartSpeed;
    public float maxSpeed;
}
