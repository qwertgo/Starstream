using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputFetcher : MonoBehaviour, TrackerInputAction.IViveTrackerActions
{
    private enum InputMethod {ViveTracker, Keyboard}

    [HideInInspector] public Vector2 planarVelocity;
    [HideInInspector] public float angularVelocity;

    [SerializeField] private InputMethod inputMethod;

    [SerializeField] private float stickDeadZone = .05f;
    [SerializeField] private float headDeadZone = .01f;
    [SerializeField] private float stickInputMultiplier = 2.5f;
    [SerializeField] private float headInputMultiplier = 10;

    [SerializeField] private Transform headTracker;
    [SerializeField] private Transform stickTracker;
    [SerializeField] private Transform inputVisualization;
    [SerializeField] private Material greenMat;
    [SerializeField] private PlayerController playerController;

    private Vector3 stickStartPosition;
    private Vector3 headStartPosition;
    private Quaternion headStartRotation;
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

        stickStartPosition = stickTracker.position;
        headStartPosition = headTracker.position;
        headStartRotation = headTracker.rotation;

        inputVisualization.GetComponent<MeshRenderer>().material = greenMat;
        GetComponentInChildren<MeshRenderer>().material = greenMat;

        playerController.inputFetcher = this;
        playerController.Go();

        if (inputMethod == InputMethod.ViveTracker)
            StartCoroutine(TrackerUpdate());
        else
            StartCoroutine(KeyboardUpdate());
    }

    private IEnumerator TrackerUpdate()
    {
        while (enabled)
        {
            float x = stickTracker.position.x - stickStartPosition.x;
            float y = stickTracker.position.y - stickStartPosition.y;

            planarVelocity = new Vector2(x, y);

            if (planarVelocity.magnitude < stickDeadZone)
                planarVelocity = Vector2.zero;
            else
                planarVelocity = (planarVelocity.magnitude - stickDeadZone) * stickInputMultiplier * planarVelocity.normalized;

            x = headTracker.position.x - headStartPosition.x;
            if (Mathf.Abs(x) < headDeadZone)
                angularVelocity = 0;
            else
                angularVelocity = Mathf.Clamp((Mathf.Abs(x) - headDeadZone) * Mathf.Sign(x) * headInputMultiplier, -1, 1);

            yield return null;
        }
    }

    private IEnumerator KeyboardUpdate()
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

            if (Input.GetKey(KeyCode.E))
                angularVelocity = 1;
            else if (Input.GetKey(KeyCode.Q))
                angularVelocity = -1;
            else
                angularVelocity = 0;

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
