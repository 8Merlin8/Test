using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCF : MonoBehaviour
{
    public Transform playerTransform;
    public float rotationSpeed = 5f;
    public float heightOffset = 1.5f;
    public float maxVerticalAngle = 60f;
    public float minVerticalAngle = -60f;

    private bool isRotating = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        HandleMouseInput();
        HandleKeyboardInput();
    }

    void HandleMouseInput()
    {
        // Если зажата правая кнопка мыши
        if (Input.GetMouseButtonDown(1))
        {
            isRotating = true;
        }

        // Если отпущена правая кнопка мыши
        if (Input.GetMouseButtonUp(1))
        {
            isRotating = false;
        }
    }

    void HandleKeyboardInput()
    {
        // Если правая кнопка мыши не зажата
        if (!isRotating)
        {
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");

            // Если есть ввод с клавиатуры
            if (horizontalInput != 0 || verticalInput != 0)
            {
                // Проверяем, нажата ли одна из стрелок клавиатуры
                if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow))
                {
                    float angle = Mathf.Atan2(horizontalInput, verticalInput);
                    float angleDegrees = angle * Mathf.Rad2Deg + 90f;
                    Quaternion rotation = Quaternion.Euler(0f, angleDegrees, 0f);

                    // Новая позиция с учетом высоты камеры
                    Vector3 targetPosition = playerTransform.position - rotation * Vector3.forward * rotationSpeed + Vector3.up * heightOffset;

                    // Плавно перемещаем камеру к новой позиции
                    transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * 5f);

                    // Направляем камеру на игрока
                    transform.LookAt(playerTransform.position);
                }
            }
        }
    }
}