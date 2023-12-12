using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolfController : MonoBehaviour
{
    public Transform ball; // ������ �� ��������� ����
    public float hitPower = 10f; // ���� �����
    public float maxHitPower = 20f; // ������������ ���� �����
    public float hitPowerIncreaseRate = 2f; // �������� ���������� ���� �����

    private bool isAiming = false; // ���� ��� �����������, ���� �� � ������ ������ ������������
    private Vector3 initialScale; // �������� ������ ����� �����
    private float hitPowerScale = 1f; // ������� ���� �����

    void Start()
    {
        initialScale = transform.localPosition;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isAiming = true;
        }

        if (isAiming)
        {
            hitPowerScale = Mathf.PingPong(Time.time * hitPowerIncreaseRate, 1f);
            transform.localPosition = new Vector3(initialScale.x, initialScale.y * hitPowerScale, initialScale.z);

            if (Input.GetMouseButtonUp(0))
            {
                // ����������� ���� ����� � ����������� �� ������� �����
                hitPower = maxHitPower * hitPowerScale;

                // ������ ���� � �������������� ���� �����
                Vector3 force = transform.forward * hitPower;
                ball.GetComponent<Rigidbody>().AddForce(force, ForceMode.Impulse);

                // ����� ����� ������������ � ����� ������� �����
                isAiming = false;
                transform.localPosition = initialScale;
            }
        }
    }
}