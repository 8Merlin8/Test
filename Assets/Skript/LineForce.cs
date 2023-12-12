using UnityEngine;

public class LineForce : MonoBehaviour
{
    [SerializeField] private float shotPower;
    [SerializeField] private float stopVelocity = 0.05f;
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private Transform startPosition;

    private bool isIdle;
    private bool isAiming;
    private new Rigidbody rigidbody; // Используем "new" для скрытия предупреждения

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        isAiming = false;
        lineRenderer.enabled = false;
    }

    private void FixedUpdate()
    {
        if (rigidbody.velocity.magnitude < stopVelocity)
        {
            Stop();
        }

        ProcessAim();
    }

    private void ProcessAim()
    {
        if (!isIdle)
        {
            return;
        }

        Vector3 targetPoint = CastMouseClickRay();

        DrawLine(targetPoint);

        if (Input.GetMouseButtonDown(0))
        {
            isAiming = true;
        }

        if (isAiming && Input.GetMouseButtonUp(0))
        {
            isAiming = false;
            Shoot(targetPoint);
        }
    }

    private void Shoot(Vector3 targetPoint)
    {
        lineRenderer.enabled = false;

        Vector3 direction = (targetPoint - transform.position).normalized;
        direction.y = 0f; // Убираем компоненту по оси Y

        // Set gravity to false while aiming
        rigidbody.useGravity = false;

        rigidbody.AddForce(direction * shotPower, ForceMode.Impulse);

        // Reset gravity after the shot
        rigidbody.useGravity = true;
        isIdle = false;
    }

    private void DrawLine(Vector3 targetPoint)
    {
        // Обновляем позицию линии в каждом кадре
        Vector3 horizontalTargetPoint = new Vector3(targetPoint.x, transform.position.y, targetPoint.z);
        Vector3[] positions = {
            transform.position,
            horizontalTargetPoint
        };
        lineRenderer.SetPositions(positions);
        lineRenderer.enabled = true;
    }

    private void Stop()
    {
        rigidbody.velocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;
        isIdle = true;
    }

    private Vector3 CastMouseClickRay()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, float.PositiveInfinity))
        {
            return hit.point;
        }
        else
        {
            return transform.position; // Возвращаем текущую позицию мяча, если луч не попал
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Cube"))
        {
            // Телепортировать мяч на стартовую позицию
            transform.position = startPosition.position;
            rigidbody.velocity = Vector3.zero;
            rigidbody.angularVelocity = Vector3.zero;
            isIdle = true;
        }
    }
}
