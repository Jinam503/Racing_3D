using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedReducer : MonoBehaviour
{
    public float reductionFactor = 0.5f;  // 속도를 얼마나 감소시킬지 결정하는 계수

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Car"))    
        {
            Car car = other.gameObject.GetComponentInParent<Car>();

            if (car != null)
            {
                foreach (var wheel in car.wheels)
                {
                    wheel.wheelCollider.brakeTorque = 2000 * car.breakAcceleration * Time.deltaTime;
                }
            }
        }
    }
}