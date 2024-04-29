using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputFetcher : MonoBehaviour, TrackerInputAction.IViveTrackerActions
{
    private enum InputMethod {SingleViveTracker, MultipleViveTracker, Keyboard}

    [HideInInspector] public Vector2 planarVelocity;
    [SerializeField] private InputMethod inputMethod;
    [SerializeField] private float stickInputMultiplier = 2.5f;
    [SerializeField] private float calibrationTime = 1.5f;

    [SerializeField] private Transform headTracker;
    [SerializeField] private Transform stickTracker;
    [SerializeField] private Transform inputVisualization;
    [SerializeField] private Material greenMat;
    [SerializeField] private PlayerController playerController;

    private Vector3 stickStartPosition;
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
        yield return new WaitForSeconds(calibrationTime);

        stickStartPosition = stickTracker.position;

        inputVisualization.GetComponent<MeshRenderer>().material = greenMat;
        GetComponentInChildren<MeshRenderer>().material = greenMat;

        playerController.inputFetcher = this;
        playerController.Go();

        switch (inputMethod)
        {
            case InputMethod.SingleViveTracker:
                StartCoroutine(SingleTrackerFetcher());
                break;
            case InputMethod.MultipleViveTracker:
                StartCoroutine(MultipleTrackerFetcher());
                break;
            default:
                StartCoroutine(KeyboardFetcher());
                break;
        }
    }

    private IEnumerator SingleTrackerFetcher()
    {
        while (enabled)
        {
            planarVelocity = stickTracker.position - stickStartPosition;

            TrackerVelocityAdjustment();

            yield return null;
        }
    }

    private IEnumerator MultipleTrackerFetcher()
    {
        while (enabled)
        {
            float x = stickTracker.position.x - stickStartPosition.x;
            float y = stickTracker.position.y - headTracker.position.y;

            planarVelocity = new Vector2(x, y);

            TrackerVelocityAdjustment();

            yield return null;
        }
    }

    private void TrackerVelocityAdjustment()
    {
        planarVelocity = planarVelocity.magnitude  * stickInputMultiplier * planarVelocity.normalized;
        planarVelocity = Vector2.ClampMagnitude(planarVelocity, 1);

        //float magnitude = planarVelocity.magnitude;
        //planarVelocity = magnitude * magnitude * planarVelocity.normalized;
    }

    //Only meant for debugging (its really bare bones)
    private IEnumerator KeyboardFetcher()
    {
        while (enabled)
        {
            planarVelocity = Vector2.zero;

            if (Input.GetKey(KeyCode.W))
                planarVelocity += Vector2.up;
            else if (Input.GetKey(KeyCode.S))
                planarVelocity += Vector2.down;

            if (Input.GetKey(KeyCode.D))
                planarVelocity += Vector2.right;
            else if (Input.GetKey(KeyCode.A))
                planarVelocity += Vector2.left;

            yield return null;
        }
    }

    #region Fetch Input from Vive Trackers
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
