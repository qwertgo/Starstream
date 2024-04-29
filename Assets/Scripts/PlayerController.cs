using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [HideInInspector] public InputFetcher inputFetcher;

    [SerializeField] private float forwardAcceleration = 1;
    [SerializeField] private float maxSpeed = 35;
    [SerializeField] private float rotationSpeed = 90;
    [SerializeField] private float avoidTubeMinDistance = 10;
    [SerializeField] private float avoidTubeMaxDistance = 100;
    [SerializeField] private float lookAtDistance = .5f;

    [SerializeField] private LayerMask tubeLayer;
    [SerializeField] private Transform lookAtTransform;

    private float currentSpeed;
    private float avoidTubePercentage;

    private Rigidbody rb;
    private Quaternion lerpToGoal;
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
            AvoidTube();
            Rotate();
            Accelerate();
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

    private void Rotate()
    {
        float lockSteeringPercentage = 1 - avoidTubePercentage;
        Quaternion rotation = Quaternion.Euler(inputFetcher.planarVelocity.x * rotationSpeed * lockSteeringPercentage * Vector3.up);
        rotation *= Quaternion.Euler(-inputFetcher.planarVelocity.y * rotationSpeed * lockSteeringPercentage * Vector3.right);
        rb.rotation *= rotation;
        
        lookAtTransform.position =
            transform.position + transform.forward * 3 + transform.rotation * inputFetcher.planarVelocity * -lookAtDistance;
    }

    //To prevent collision redirect player if he steers into the tube
    private void AvoidTube()
    {
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, avoidTubeMaxDistance, tubeLayer))
        {
            float rayLength = (hit.point - transform.position).magnitude;
            avoidTubePercentage = (rayLength - avoidTubeMinDistance) / (avoidTubeMaxDistance - avoidTubeMinDistance);
            
            Vector3 newForward = Vector3.ProjectOnPlane(transform.forward, hit.normal);
            lerpToGoal = Quaternion.LookRotation(newForward, transform.up);
            Quaternion maxTubeAvoidance = Quaternion.Lerp(rb.rotation, lerpToGoal,  Time.deltaTime);

            rb.rotation = Quaternion.Lerp(rb.rotation, maxTubeAvoidance, avoidTubePercentage);
        }
        else
        {
            avoidTubePercentage = 0;
        }

        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer != 3)
        {
            Debug.Log(collision.gameObject.layer);
            return;
        }

        Vector3 contactPoint = collision.contacts[0].point;
        Vector3 vecToContact = contactPoint - transform.position;
        Physics.Raycast(transform.position, vecToContact.normalized, out RaycastHit hit, 10, tubeLayer);

        Vector3 newForward = Vector3.ProjectOnPlane(transform.forward, hit.normal);
        rb.rotation = Quaternion.LookRotation(newForward, transform.up);
    }
}
