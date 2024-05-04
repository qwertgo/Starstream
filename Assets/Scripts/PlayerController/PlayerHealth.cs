using System.Collections;
using System.Collections.Generic;
using SOEvent.Sender;
using UnityEngine;
using UnityEngine.VFX;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    int playerHealth = 5;
    [SerializeField]List<GameObject> healthLamps;
    [SerializeField]GameEvent playerDeadEvent;
    [SerializeField]VisualEffect explosionVFX;
    

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
            VisualEffect exploVFX = Instantiate(explosionVFX,transform.position,transform.rotation);
            exploVFX.transform.localScale *= .7f;
            exploVFX.transform.localPosition = exploVFX.transform.position;
            // return;
        }
        
        healthLamps[0].GetComponent<BlinkingLights>().EnableLamp();
        healthLamps.Remove(healthLamps[0]);
        
        // if(playerHealth <= 0)
        //     playerDeadEvent.TriggerEvent();
    }
}
