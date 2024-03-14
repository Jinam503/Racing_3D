using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MovingCube : MonoBehaviour
{
    public float gravityMagnitude;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        Vector3 gravityDirection = -Vector3.up;
        Vector3 gravity = gravityDirection * Physics.gravity.magnitude * gravityMagnitude;
        rb.AddForce(gravity, ForceMode.Acceleration); 
    }
}
