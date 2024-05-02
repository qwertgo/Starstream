using System.Collections;
using System.Collections.Generic;
using SOEvent.Sender;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    int playerHealth = 5;
    [SerializeField]List<GameObject> healthLamps;
    [SerializeField]GameEvent playerDeadEvent;

    public void LoseHealth()
    {
        playerHealth -= 1;
        UpdateHealthDisplay();
    }

    public void UpdateHealthDisplay()
    {
        if(playerHealth <= 0)
        {
            playerDeadEvent.TriggerEvent();
            return;
        }
        healthLamps[0].GetComponent<BlinkingLights>().EnableLamp();
        healthLamps.Remove(healthLamps[0]);
        if(playerHealth <= 0)
            playerDeadEvent.TriggerEvent();
    }
}
