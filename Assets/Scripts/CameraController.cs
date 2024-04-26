using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform lookAtTransform;

    private void Update()
    {
        transform.rotation = transform.parent.rotation;
        transform.LookAt(lookAtTransform, transform.up);
    }
}