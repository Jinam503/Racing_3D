using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddArtificialGravity : MonoBehaviour
{
    public GravityType gravityType;
    public float gravityScale;
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("MovingCube"))
        {
            switch (gravityType)
            {
                case GravityType.Right:
                    other.GetComponent<Rigidbody>().AddForce(Vector3.right * gravityScale, ForceMode.Force);
                    break;
                case GravityType.Left:
                    other.GetComponent<Rigidbody>().AddForce(Vector3.left * gravityScale, ForceMode.Force);
                    break;
                case GravityType.Up:
                    other.GetComponent<Rigidbody>().AddForce(Vector3.up * gravityScale, ForceMode.Force);
                    break;
                case GravityType.Down:
                    other.GetComponent<Rigidbody>().AddForce(Vector3.down * gravityScale, ForceMode.Force);
                    break;
                case GravityType.Front:
                    other.GetComponent<Rigidbody>().AddForce(Vector3.forward * gravityScale, ForceMode.Force);
                    break;
                case GravityType.Back:
                    other.GetComponent<Rigidbody>().AddForce(Vector3.back * gravityScale, ForceMode.Force);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
    }
}

public enum GravityType
{
    Right,
    Left,
    Up,
    Down,
    Front,
    Back,
}
