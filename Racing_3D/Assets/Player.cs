using UnityEngine;

public class Player : MonoBehaviour
{
    public float acceleration = 2f; // 가속도
    public float maxSpeed = 10f; // 최대 속도
    public float rotationSpeed = 100f; // 회전 속도

    private CharacterController characterController;
    private float currentSpeed = 0f;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        // 앞으로 이동
        float verticalInput = Input.GetAxis("Vertical");

        // 가속도 적용
        if (verticalInput > 0f)
        {
            currentSpeed += acceleration * Time.deltaTime;
        }
        else
        {
            currentSpeed = Mathf.MoveTowards(currentSpeed, 0f, acceleration * Time.deltaTime);
        }

        // 속도 제한
        currentSpeed = Mathf.Clamp(currentSpeed, 0f, maxSpeed);

        // 이동 적용
        characterController.Move(transform.forward * currentSpeed * Time.deltaTime);

        // 좌우로 회전
        float horizontalInput = Input.GetAxis("Horizontal");
        Vector3 rotation = new Vector3(0f, horizontalInput * rotationSpeed * Time.deltaTime, 0f);
        transform.Rotate(rotation);
    }
}