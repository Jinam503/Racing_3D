using System;
using System.Collections;
using System.Collections.Generic;
using Palmmedia.ReportGenerator.Core.Reporting.Builders;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

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
        public GameObject wheelEffectObj;
        public ParticleSystem smokeParticle;
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

    public TextMeshProUGUI speed;

    public float currentSpeed;
    public float maxSpeed;
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
        GetInputs();
        AnimateWheels();
        WheelEffects();

    }
    private void LateUpdate()
    {
        Move();
        Steer();
        Brake();
    }

    private void GetInputs()
    {
        moveInput = Input.GetAxis("Vertical");
        steerInput = Input.GetAxis("Horizontal");
    }
    private void AnimateWheels()
    {
        foreach (var wheel in wheels)
        {
            Quaternion rot;
            Vector3 pos;
            wheel.wheelCollider.GetWorldPose(out pos, out rot);
            wheel.wheelModel.transform.position = pos;
            wheel.wheelModel.transform.rotation = rot;
        }
    }
    private void WheelEffects()
    {
        foreach (var wheel in wheels)
        {
            //var dirtParticleMainSettings = wheel.smokeParticle.main;

            if (Input.GetKey(KeyCode.Space) && wheel.axel == Axel.Rear && wheel.wheelCollider.isGrounded == true && carRigidBody.velocity.magnitude >= 10.0f)
            {
                wheel.wheelEffectObj.GetComponentInChildren<TrailRenderer>().emitting = true;
                wheel.smokeParticle.Emit(1);
            }
            else
            {
                wheel.wheelEffectObj.GetComponentInChildren<TrailRenderer>().emitting = false;
            }
        }
    }
    private void Move()
    {
        foreach (var wheel in wheels)
        {
            wheel.wheelCollider.motorTorque = moveInput * moveSpeed * maxAcceleration * Time.deltaTime;
        }
        
        currentSpeed = carRigidBody.velocity.magnitude;

        // 최대 속도를 넘으면 속도를 제한
        if (currentSpeed > maxSpeed)
        {
            // 현재 속도의 방향을 유지한 채로 최대 속도로 조절
            carRigidBody.velocity = carRigidBody.velocity.normalized * maxSpeed;
        }
    }
    private void Steer()
    {
        foreach (var wheel in wheels)
        {
            if (wheel.axel == Axel.Front)
            {
                float steerAngle = steerInput * turnVelocity * maxSteerAngle;
                wheel.wheelCollider.steerAngle = Mathf.Lerp(wheel.wheelCollider.steerAngle, steerAngle, 0.6f);
            }
        }
    }
    private void Brake()
    {
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

    private IEnumerator SpeedUP()
    {
        moveSpeed = 3000;

        float t = 0f, speed;

        while (t < 3f)
        {   
            speed = Mathf.Lerp(3000, 1000, t / 2f);
            moveSpeed = speed;
            t += Time.deltaTime;
            yield return null;
        }
    }

}
