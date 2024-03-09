using System;
using System.Collections;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform car;
    public Vector3 moveOffset;
    public Vector3 rotOffset;
    
    public float moveSmoothness;
    public float rotSmoothness;
    private void FixedUpdate()
    {
        Vector3 pos = new Vector3();
        pos = car.TransformPoint(moveOffset);

        transform.position = Vector3.Lerp(transform.position, pos, moveSmoothness * Time.deltaTime);

        var direction = car.position - transform.position;
        var rotation = new Quaternion();
        rotation = Quaternion.LookRotation(direction + rotOffset, Vector3.up);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, rotSmoothness * Time.deltaTime);
    }
}