using UnityEngine;

public class LineForce : MonoBehaviour
{
    [SerializeField] private float shotPower;
    [SerializeField] private float stopVelocity = 0.05f;
    [SerializeField] private LineRenderer lineRenderer;

    private bool isIdle = true;
    private bool isAiming = false;
    private Rigidbody rb;
    private Vector3 startPosition;
    private Vector3 lastStopPosition;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        lineRenderer.enabled = false;
        startPosition = lastStopPosition = transform.position;
    }

    private void FixedUpdate()
    {
        if (rb.velocity.magnitude < stopVelocity) Stop();
        ProcessAim();
    }

    private void OnMouseDown()
    {
        if (isIdle) isAiming = true;
    }

    private void ProcessAim()
    {
        if (!isAiming || !isIdle) return;

        Vector3? worldPoint = CastMouseClickRay();

        if (worldPoint != null)
        {
            DrawLine(new Vector3(worldPoint.Value.x, transform.position.y, worldPoint.Value.z));

            if (Input.GetMouseButtonUp(0)) Shoot(new Vector3(worldPoint.Value.x, transform.position.y, worldPoint.Value.z));
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Water")) ReturnToLastStopPosition();
    }

    private void Shoot(Vector3 worldPoint)
    {
        isAiming = false;
        lineRenderer.enabled = false;

        Vector3 direction = (worldPoint - transform.position).normalized;
        float strength = Vector3.Distance(transform.position, worldPoint);

        rb.AddForce(new Vector3(direction.x, 0f, direction.z) * strength * shotPower);
        isIdle = false;
    }

    private void DrawLine(Vector3 worldPoint)
    {
        lineRenderer.SetPositions(new[] { transform.position, worldPoint });
        lineRenderer.enabled = true;
    }

    private void Stop()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        lastStopPosition = transform.position;
        isIdle = true;
    }

    private Vector3? CastMouseClickRay()
    {
        Vector3 screenMousePosFar = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.farClipPlane);
        Vector3 screenMousePosNear = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane);
        Vector3 worldMousePosFar = Camera.main.ScreenToWorldPoint(screenMousePosFar);
        Vector3 worldMousePosNear = Camera.main.ScreenToWorldPoint(screenMousePosNear);

        if (Physics.Raycast(worldMousePosNear, worldMousePosFar - worldMousePosNear, out var hit, float.PositiveInfinity))
            return new Vector3(hit.point.x, 0f, hit.point.z);

        return null;
    }

    private void ReturnToLastStopPosition()
    {
        transform.position = lastStopPosition;
        Stop();
    }
}
