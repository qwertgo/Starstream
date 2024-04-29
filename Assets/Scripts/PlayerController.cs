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

    [SerializeField] private float avoidTubeRaycastRotation = 20; // how much to rotate the 4 raycast away from the forward vector

    [SerializeField] private LayerMask tubeLayer;
    [SerializeField] private Transform lookAtTransform;

    private float currentSpeed;
    private float avoidTubePercentage;

    private Rigidbody rb;
    private Camera cam;
    private Vector2 avoidTubeInput;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        cam = Camera.main;
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
        Quaternion minTubeAvoidance = Quaternion.Euler(inputFetcher.planarVelocity.x * rotationSpeed  * Vector3.up);
        minTubeAvoidance *= Quaternion.Euler(-inputFetcher.planarVelocity.y * rotationSpeed  * Vector3.right);
        minTubeAvoidance = rb.rotation * minTubeAvoidance;
        
        Quaternion maxTubeAvoidance = Quaternion.Euler(avoidTubeInput.x * rotationSpeed  * Vector3.up);
        maxTubeAvoidance *= Quaternion.Euler(-avoidTubeInput.y * rotationSpeed * Vector3.right);
        maxTubeAvoidance = rb.rotation * maxTubeAvoidance;

        rb.rotation = Quaternion.Lerp(minTubeAvoidance, maxTubeAvoidance, avoidTubePercentage);
        
        lookAtTransform.position =
            transform.position + transform.forward * 3 + transform.rotation * inputFetcher.planarVelocity * -lookAtDistance;
    }

    //To prevent collision redirect player if he steers into the tube
    private void AvoidTube()
    {
        int rayCastHits = 0;
        
        Vector3 ray1 = Quaternion.Euler(avoidTubeRaycastRotation * transform.up) * transform.forward;
        Vector3 ray2 = Quaternion.Euler(-avoidTubeRaycastRotation * transform.up) * transform.forward;
        Vector3 ray3 = Quaternion.Euler(avoidTubeRaycastRotation * transform.right) * transform.forward;
        Vector3 ray4 = Quaternion.Euler(-avoidTubeRaycastRotation * transform.right) * transform.forward;
        
        // Debug.DrawRay(transform.position, ray1 * avoidTubeMaxDistance, Color.blue);
        // Debug.DrawRay(transform.position, ray2 * avoidTubeMaxDistance, Color.blue);
        // Debug.DrawRay(transform.position, ray3 * avoidTubeMaxDistance, Color.red);
        // Debug.DrawRay(transform.position, ray4 * avoidTubeMaxDistance, Color.red);

        rayCastHits += RayCastSumIteration(ray1);
        rayCastHits += RayCastSumIteration(ray2);
        rayCastHits += RayCastSumIteration(ray3);
        rayCastHits += RayCastSumIteration(ray4);

        if (rayCastHits == 0)
        {
            avoidTubePercentage = 0;
            avoidTubeInput = Vector2.zero;
        }
        else
        {
            avoidTubePercentage /= rayCastHits;
            avoidTubePercentage *= avoidTubePercentage;
            avoidTubeInput /= rayCastHits;
            avoidTubeInput.Normalize();
        }
    }

    private int RayCastSumIteration(Vector3 dir)
    {
        if (Physics.Raycast(transform.position, dir, out RaycastHit hit, avoidTubeMaxDistance, tubeLayer))
        {
            float rayLength = (hit.point - transform.position).magnitude;
            avoidTubePercentage += 1 - (rayLength - avoidTubeMinDistance) / (avoidTubeMaxDistance - avoidTubeMinDistance);
            
            Vector3 p1 = cam.WorldToScreenPoint(hit.point);
            Vector3 p2 = cam.WorldToScreenPoint(hit.point - hit.normal);
            avoidTubeInput += ((Vector2)(p1 - p2)).normalized;

            return 1;
        }

        return 0;
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
