using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputFetcher : MonoBehaviour, TrackerInputAction.IViveTrackerActions
{
    [SerializeField] private float deadZoneWidth = .2f;
    [SerializeField] private float inputMultiplier = 2.5f;

    [SerializeField] private Transform tracker1;
    [SerializeField] private Transform tracker2;
    [SerializeField] private Transform directionTest;
    [SerializeField] private Material greenMat;

    TrackerInputAction controls;

    private Vector3 startPosition;


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
        startPosition = tracker2.position;
        directionTest.GetComponent<MeshRenderer>().material = greenMat;
    }

    void Update()
    {
        float xOffset = (tracker2.position.x - startPosition.x);
        xOffset = Mathf.Clamp01(Mathf.Abs(xOffset * inputMultiplier) - deadZoneWidth) * Mathf.Sign(xOffset);
        float yOffset =  tracker2.position.y - startPosition.y;
        yOffset = Mathf.Clamp01(Mathf.Abs(yOffset * inputMultiplier) - deadZoneWidth) * Mathf.Sign(yOffset);

        Vector2 offset = new Vector2(xOffset, yOffset);
        directionTest.position = (Vector3)offset + new Vector3(0, 2, 0);
    }

    #region Input
    public void OnPosition_1(InputAction.CallbackContext context)
    {
        tracker1.position = context.ReadValue<Vector3>() ;
    }

    public void OnRotation_1(InputAction.CallbackContext context)
    {
        tracker1.rotation = context.ReadValue<Quaternion>();
    }

    public void OnPosition_2(InputAction.CallbackContext context)
    {
        tracker2.position = context.ReadValue<Vector3>();
    }

    public void OnRotation_2(InputAction.CallbackContext context)
    {
        tracker2.rotation = context.ReadValue<Quaternion>();
    }


    #endregion
}
