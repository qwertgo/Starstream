using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleCameraController : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float rotationSpeed;

    private void Update()
    {
        if (Input.GetKey(KeyCode.W))
            transform.position += Time.deltaTime * speed * transform.forward;
        else if (Input.GetKey(KeyCode.S))
            transform.position -= Time.deltaTime * speed * transform.forward;
        
        if (Input.GetKey(KeyCode.D))
            transform.position += Time.deltaTime * speed * transform.right;
        else if (Input.GetKey(KeyCode.A))
            transform.position -= Time.deltaTime * speed * transform.right;

        if (Input.GetKey(KeyCode.E))
            transform.position += Time.deltaTime * speed * transform.up;
        else if (Input.GetKey(KeyCode.Q))
            transform.position -= Time.deltaTime * speed * transform.up;

        if (Input.GetKey(KeyCode.RightArrow))
            transform.rotation *= Quaternion.Euler(Time.deltaTime * rotationSpeed * transform.up);
        else if (Input.GetKey(KeyCode.LeftArrow))
            transform.rotation *= Quaternion.Euler(Time.deltaTime * rotationSpeed * -transform.up);
        
        if (Input.GetKey(KeyCode.UpArrow))
            transform.rotation *= Quaternion.Euler(Time.deltaTime * rotationSpeed * transform.right);
        else if (Input.GetKey(KeyCode.DownArrow))
            transform.rotation *= Quaternion.Euler(Time.deltaTime * rotationSpeed * -transform.right);
    }
}
