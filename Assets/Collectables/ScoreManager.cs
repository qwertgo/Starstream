using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    int score = 0;
    private void Start() 
    {
        
    }
    public void AddScore() => score++;
}
