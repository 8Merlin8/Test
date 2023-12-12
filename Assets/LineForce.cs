using UnityEngine;

public class LineForce : MonoBehaviour
{
    [SerializeField] private float shotPower;
    [SerializeField] private float stopVelocity = 0.05f; // The velocity below which the rigidbody will be considered as stopped
    [SerializeField] private LineRenderer lineRenderer;

    private bool isIdle;
    private bool isAiming;
    private Rigidbody rigidbody;

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

    private void OnMouseDown()
    {
        if (isIdle)
        {
            isAiming = true;
        }
    }

    private void ProcessAim()
    {
        if (!isAiming || !isIdle)
        {
            return;
        }

        Vector3? worldPoint = CastMouseClickRay();

        if (!worldPoint.HasValue)
        {
            return;
        }

        DrawLine(worldPoint.Value);

        if (Input.GetMouseButtonUp(0))
        {
            if (worldPoint != null)
            {
                Shoot(worldPoint.Value);
            }
        }
    }

    private void Shoot(Vector3 worldPoint)
    {
        isAiming = false;
        lineRenderer.enabled = false;

        Vector3 horizontalWorldPoint = new Vector3(worldPoint.x, transform.position.y, worldPoint.z);
        Vector3 direction = (horizontalWorldPoint - transform.position).normalized;
        float strength = Vector3.Distance(transform.position, horizontalWorldPoint);

        rigidbody.AddForce(direction * strength * shotPower);
        isIdle = false;
    }

    private void DrawLine(Vector3 worldPoint)
    {
        Vector3[] positions = {
            transform.position,
            worldPoint
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

    private Vector3? CastMouseClickRay()
    {
        Camera mainCamera = Camera.main;

        if (mainCamera == null)
        {
            Debug.LogError("Main camera not found.");
            return null;
        }

        Vector3 screenMousePosFar = new Vector3(
            Input.mousePosition.x,
            Input.mousePosition.y,
            mainCamera.farClipPlane);
        Vector3 screenMousePosNear = new Vector3(
            Input.mousePosition.x,
            Input.mousePosition.y,
            mainCamera.nearClipPlane);
        Vector3 worldMousePosFar = mainCamera.ScreenToWorldPoint(screenMousePosFar);
        Vector3 worldMousePosNear = mainCamera.ScreenToWorldPoint(screenMousePosNear);
        RaycastHit hit;

        if (Physics.Raycast(worldMousePosNear, worldMousePosFar - worldMousePosNear, out hit, float.PositiveInfinity))
        {
            return hit.point;
        }
        else
        {
            return null;
        }
    }
}
