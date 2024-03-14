using System;
using System.Collections;
using System.Collections.Generic;
using Palmmedia.ReportGenerator.Core.Reporting.Builders;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
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

    public bool extraWheel;
    public bool _6engine;
    public bool _8engine;
    public bool boost;
    public bool rocket;
    
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
        Items();

        speed.text = Mathf.RoundToInt(carRigidBody.velocity.magnitude).ToString();
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
    private void Items()
    {
        if (_8engine)
        {
            maxSpeed = 50;
        }
        else if (_6engine) maxSpeed = 40;
        else maxSpeed = 30;

        if (Input.GetKeyDown(KeyCode.F))
        {
            carRigidBody.AddForce(transform.forward * 3000, ForceMode.Impulse);
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
}
