using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputFetcher : MonoBehaviour, TrackerInputAction.IViveTrackerActions
{
    [SerializeField] private Transform tracker1;
    [SerializeField] private Transform tracker2;

    TrackerInputAction controls;


    // Start is called before the first frame update
    void Start()
    {
        if(controls == null)
        {
            controls = new TrackerInputAction();
            controls.Enable();
            controls.ViveTracker.SetCallbacks(this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region Input
    public void OnPosition_1(InputAction.CallbackContext context)
    {
        //tracker1.position = context.ReadValue<Vector3>();
    }

    public void OnRotation_1(InputAction.CallbackContext context)
    {
        tracker1.rotation = context.ReadValue<Quaternion>();
    }
    #endregion
}
