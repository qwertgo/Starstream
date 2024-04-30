using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [HideInInspector] public InputFetcher inputFetcher;

    [SerializeField] private Difficulty baseDifficulty;
    [SerializeField] private Difficulty harderDifficulty;
    private float forwardAcceleration = 1;
    private float startSpeed = 30;
    private float startMaxSpeed = 60;
    private float timeForStartSpeed = 10;
    private float maxSpeed = 35;
    [SerializeField] private float rotationSpeed = 90;
    [SerializeField] private float avoidTubeMinDistance = 10;
    [SerializeField] private float avoidTubeMaxDistance = 100;
    [SerializeField] private float lookAtDistance = .5f;

    [SerializeField] private float avoidTubeRaycastRotation = 20; // how much to rotate the 4 raycast away from the forward vector

    [SerializeField] private LayerMask tubeLayer;
    [SerializeField] private Transform lookAtTransform;
    [SerializeField] private GameObject speedLinesParticleSystem;

    private float currentSpeed;
    private float avoidTubePercentage;
    private float wrongDirectionTimer;
    private float currentMaxSpeed;

    private Rigidbody rb;
    private Camera cam;
    private Vector2 avoidTubeInput;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        cam = Camera.main;

        ApplyDifficulty(baseDifficulty);
    }

    private void ApplyDifficulty(Difficulty difficulty)
    {
        forwardAcceleration = difficulty.forwardAcceleration;
        startSpeed = difficulty.startSpeed;
        startMaxSpeed = difficulty.startMaxSpeed;
        timeForStartSpeed = difficulty.timeForStartSpeed;
        maxSpeed = difficulty.maxSpeed;
    }
    public void Go()
    {
        speedLinesParticleSystem.SetActive(true);
        currentSpeed = startSpeed;
        rb.velocity = transform.forward * currentSpeed;
        currentMaxSpeed = startMaxSpeed;
        
        StartCoroutine(WaitForMaxSpeedChange());
        StartCoroutine(UpdateCoroutine());
    }

    private IEnumerator WaitForMaxSpeedChange()
    {
        yield return new WaitForSeconds(timeForStartSpeed);
        currentMaxSpeed = maxSpeed;
    }

    private IEnumerator UpdateCoroutine()
    {
        while (enabled)
        {
            AvoidTube();
            Rotate();
            Accelerate();
            CheckIfHeadingInRightDirection();
            yield return null;
        }

        rb.velocity = Vector3.zero;
    }

    private void Accelerate()
    {
        currentSpeed += Time.deltaTime * forwardAcceleration;
        currentSpeed = Mathf.Min(currentSpeed, currentMaxSpeed);

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

    #region Avoid Tube
    //To prevent collision redirect player if he steers into the tube
    //actual avoidance is in the rotate function. This precalculates data which is used later
    private void AvoidTube()
    {
        int rayCastHits = 0;
        
        //Rotate forward vector in 4 directions
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
            //avoidTubePercentage = Mathf.SmoothStep(0, 1, avoidTubePercentage);
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
            
            //convert hit normal to screen space
            Vector3 p1 = cam.WorldToScreenPoint(hit.point);
            Vector3 p2 = cam.WorldToScreenPoint(hit.point - hit.normal);
            avoidTubeInput += ((Vector2)(p1 - p2)).normalized;

            return 1;
        }

        return 0;
    }
    #endregion

    private void CheckIfHeadingInRightDirection()
    {
        if (Vector3.Dot(Vector3.forward, transform.forward) < -.5f)
        {
            wrongDirectionTimer += Time.deltaTime;
            if(wrongDirectionTimer > 5)
                ResetPlayer();
        }
        else
            wrongDirectionTimer = 0;
    }

    private void ResetPlayer()
    {
        rb.rotation = Quaternion.LookRotation(Vector3.forward, transform.up);
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
