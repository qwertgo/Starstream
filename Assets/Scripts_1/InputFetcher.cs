using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputFetcher : MonoBehaviour, TrackerInputAction.IViveTrackerActions
{
    [HideInInspector] public Vector2 planarVelocity;
    [HideInInspector] public float angluarVelocity;

    [SerializeField] private float deadZoneWidth = .2f;
    [SerializeField] private float inputMultiplier = 2.5f;

    [SerializeField] private Transform headTracker;
    [SerializeField] private Transform stickTracker;
    [SerializeField] private Transform inputVisualization;
    [SerializeField] private Material greenMat;
    [SerializeField] private PlayerController playerController;

    private Vector3 startPosition;
    private TrackerInputAction controls;


    // Start is called before the first frame update
    IEnumerator Start()
    {
        if(controls == null)
        {
            controls = new TrackerInputAction();
            controls.Enable();
            controls.ViveTracker.SetCallbacks(this);
        }
        yield return new WaitForSeconds(3);
        startPosition = stickTracker.position;
        inputVisualization.GetComponent<MeshRenderer>().material = greenMat;
        GetComponentInChildren<MeshRenderer>().material = greenMat;

        playerController.inputFetcher = this;
        playerController.Go();
    }

    void Update()
    {
        float xOffset = (stickTracker.position.x - startPosition.x);
        xOffset = Mathf.Clamp01(Mathf.Abs(xOffset * inputMultiplier) - deadZoneWidth) * Mathf.Sign(xOffset);

        float yOffset =  stickTracker.position.y - startPosition.y;
        yOffset = Mathf.Clamp01(Mathf.Abs(yOffset * inputMultiplier) - deadZoneWidth) * Mathf.Sign(yOffset);

        planarVelocity = new Vector2(xOffset, yOffset);
        inputVisualization.position = (Vector3)planarVelocity + new Vector3(0, 2, 0);
    }

    #region Input
    public void OnPosition_1(InputAction.CallbackContext context)
    {
        headTracker.position = context.ReadValue<Vector3>() ;
    }

    public void OnRotation_1(InputAction.CallbackContext context)
    {
        headTracker.rotation = context.ReadValue<Quaternion>();
    }

    public void OnPosition_2(InputAction.CallbackContext context)
    {
        stickTracker.position = context.ReadValue<Vector3>();
    }

    public void OnRotation_2(InputAction.CallbackContext context)
    {
        stickTracker.rotation = context.ReadValue<Quaternion>();
    }
    #endregion
}
