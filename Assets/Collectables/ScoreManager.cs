using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;  
public class ScoreManager : MonoBehaviour
{
    public static ScoreManager S;
    [ShowNonSerializedField]int score = 0;
    private void Awake() {
        if(S == null)
            S = this;
        else
            Destroy(this.gameObject);
    }
    private void Start() 
    {
        
    }
    public void AddScore() => score++;
}
