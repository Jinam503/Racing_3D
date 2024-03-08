using System;
using System.Collections;
using System.Collections.Generic;
using Palmmedia.ReportGenerator.Core.Reporting.Builders;
using UnityEngine;

public class Car : MonoBehaviour
{
    public enum Axel
    {
        Front,
        Rear,
    }
    [Serializable] public struct Wheel
    {
        public GameObject wheelModel;
        public WheelCollider wheelCollider;
        public Axel axel;
    }

    public float maxAcceleration;
    public float breakAcceleration;
    public float moveSpeed = 600f;

    public float turnVelocity = 1f;
    public float maxSteerAngle = 30f;

    public Vector3 centerOfMass;

    public List<Wheel> wheels;

    private Rigidbody carRigidBody;

    private float moveInput;
    private float steerInput;

    private void Awake()
    {
        carRigidBody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        carRigidBody.centerOfMass = centerOfMass;
    }

    private void Update()
    {
        moveInput = Input.GetAxis("Vertical");
        steerInput = Input.GetAxis("Horizontal");
        
        foreach (var wheel in wheels)
        {
            Quaternion rot;
            Vector3 pos;
            wheel.wheelCollider.GetWorldPose(out pos, out rot);
            wheel.wheelModel.transform.position = pos;
            wheel.wheelModel.transform.rotation = rot;
        }
    }

    private void LateUpdate()
    {
        foreach (var wheel in wheels)
        {
            wheel.wheelCollider.motorTorque = moveInput * moveSpeed * maxAcceleration * Time.deltaTime;
        }

        foreach (var wheel in wheels)
        {
            if (wheel.axel == Axel.Front)
            {
                float steerAngle = steerInput * turnVelocity * maxSteerAngle;
                wheel.wheelCollider.steerAngle = Mathf.Lerp(wheel.wheelCollider.steerAngle, steerAngle, 0.6f);
            }
        }

        if (Input.GetKey(KeyCode.Space))
        {
            foreach (var wheel in wheels)
            {
                wheel.wheelCollider.brakeTorque = 300 * breakAcceleration * Time.deltaTime;
            }
        }
        else
        {
            foreach (var wheel in wheels)
            {
                wheel.wheelCollider.brakeTorque = 0;
            }
        }
    }
}
