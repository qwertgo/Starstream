using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraRotator : MonoBehaviour
{
    [Header("Test")] 
    [SerializeField] private float rotationSpeed;

    private void Update()
    {
        transform.rotation *= Quaternion.Euler(Vector3.up * (rotationSpeed * Time.deltaTime));
    }

}
