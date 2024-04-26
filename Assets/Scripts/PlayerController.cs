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

    [SerializeField] private LayerMask tubeLayer;
    //[SerializeField] private Transform visuals;
    [SerializeField] private Transform lookAtTransform;
    [SerializeField] private float lookAtDistance = .5f;

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
            //PlanarMovement();
            Rotation();
            yield return null;
        }

        rb.velocity = Vector3.zero;
    }

    private void Accelerate()
    {
        currentSpeed += Time.deltaTime * forwardAcceleration;
        currentSpeed = Mathf.Min(currentSpeed, maxSpeed);

        rb.velocity = transform.forward * currentSpeed;
    }

    private void PlanarMovement()
    {
        rb.velocity += Vector3.ProjectOnPlane(inputFetcher.planarVelocity * planarSpeed, transform.forward);
    }

    private void Rotation()
    {
        //Debug.Log(inputFetcher.planarVelocity.x);
        Quaternion rotation = Quaternion.Euler(inputFetcher.planarVelocity.x * rotationSpeed * transform.up);
        rotation *= Quaternion.Euler(-inputFetcher.planarVelocity.y * rotationSpeed * Vector3.right);
        rb.rotation *= rotation;
        //visuals.localRotation = rotation;
        lookAtTransform.position =
            transform.position + transform.forward * 3 + transform.rotation * inputFetcher.planarVelocity * -lookAtDistance;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer != 3)
        {
            Debug.Log(collision.gameObject.layer);
            return;
        }

        Vector3 contacPoint = collision.contacts[0].point;
        Vector3 vecToContact = contacPoint - transform.position;
        Physics.Raycast(transform.position, vecToContact.normalized, out RaycastHit hit, 20, tubeLayer);

        Vector3 newForward = Vector3.ProjectOnPlane(transform.forward, hit.normal);
        rb.rotation = Quaternion.LookRotation(newForward, transform.up);

    }
}
