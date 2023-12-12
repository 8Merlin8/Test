using UnityEngine;

public class Skripttest1 : MonoBehaviour
{
    public float forceMultiplier = 10f;
    public float maxSpeed = 5f;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; // Чтобы избежать вращения мяча
    }

    void Update()
    {
        // Получаем ввод с мыши
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        // Создаем вектор направления на основе ввода
        Vector3 moveDirection = new Vector3(mouseX, 0, mouseY);

        // Прикладываем силу в направлении мыши
        rb.AddForce(moveDirection * forceMultiplier);

        // Ограничиваем скорость в плоскости XZ (горизонтальной)
        Vector3 horizontalVelocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        rb.velocity = Vector3.ClampMagnitude(horizontalVelocity, maxSpeed);

        // Поддерживаем мяч на одной высоте
        rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
    }
}
