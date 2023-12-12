using UnityEngine;

public class CameraVA : MonoBehaviour
{
    public Transform target;  // Сюда подставьте мяч
    public float rotationSpeed = 3f;
    public float cameraDistance = 5f;
    public float heightOffset = 2f;
    public float minVerticalAngle = -80f;
    public float maxVerticalAngle = 80f;

    private Vector3 offset;
    private bool isRotating = false;

    void Start()
    {
        offset = transform.position - target.position;
        
    }

    void Update()
    {
        HandleInput();

        if (isRotating)
        {
            RotateCamera();
        }
        else
        {
            UpdateCameraPosition();
        }
    }

    void HandleInput()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Cursor.lockState = CursorLockMode.Locked;
            isRotating = true;
        }

        if (Input.GetMouseButtonUp(1))
        {
            Cursor.lockState = CursorLockMode.None;
            isRotating = false;
        }

        
    }
void RotateCamera()
    {
        float mouseX = Input.GetAxis("Mouse X") * rotationSpeed;
            float mouseY = Input.GetAxis("Mouse Y") * rotationSpeed;

            // Вращаем камеру вокруг игрока
            transform.RotateAround(target.position, Vector3.up, mouseX);
            transform.RotateAround(target.position, transform.right, -mouseY);

            // Ограничиваем углы вращения
            float currentVerticalAngle = Vector3.Angle(transform.forward, target.position - transform.position) - 90f;
            currentVerticalAngle = Mathf.Clamp(currentVerticalAngle, minVerticalAngle, maxVerticalAngle);

            // Устанавливаем новую позицию камеры с учетом высоты
            Vector3 offsetDir = (transform.position - target.position).normalized;
            Vector3 newPosition = target.position + offsetDir * heightOffset;
            transform.position = newPosition;

            // Направляем камеру на игрока
            transform.LookAt(target.position);


            
    }
    void UpdateCameraPosition()
    {
        // Устанавливаем новую позицию камеры с учетом высоты
            Vector3 offsetDir = (transform.position - target.position).normalized;
            Vector3 newPosition = target.position + offsetDir * heightOffset;
            transform.position = newPosition;

            // Направляем камеру на игрока
            transform.LookAt(target.position);
    }
}
