using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [HideInInspector] public InputFetcher inputFetcher;

    [SerializeField] private float forwardAcceleration = 1;
    [SerializeField] private float maxSpeed = 35;
    [SerializeField] private float planarSpeed = 2;
    [SerializeField] private float rotationSpeed = 90;

    private Rigidbody rb;
    private float currentSpeed;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    public void Go()
    {
        StartCoroutine(UpdateCoroutine());
    }

    private IEnumerator UpdateCoroutine()
    {
        while (enabled)
        {
            Accelerate();
            PlanarMovement();
            yield return null;
        }

        rb.velocity = Vector3.zero;
    }

    private void Accelerate()
    {
        //currentSpeed += Time.deltaTime * forwardAcceleration;
        currentSpeed = Mathf.Max(currentSpeed);

        rb.velocity = transform.forward * currentSpeed;
    }

    private void PlanarMovement()
    {
        rb.velocity += Vector3.ProjectOnPlane(inputFetcher.planarVelocity * planarSpeed, transform.forward);
    }

    private void Rotation()
    {

    }
}
