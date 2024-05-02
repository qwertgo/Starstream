using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    int playerHealth = 5;
    [SerializeField]List<GameObject> healthLamps;

    public void LoseHealth()
    {
        playerHealth -= 1;
        UpdateHealthDisplay();
    }

    public void UpdateHealthDisplay()
    {
        healthLamps[1].GetComponent<BlinkingLight>().Play;
    }
}
