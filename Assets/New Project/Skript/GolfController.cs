using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolfController : MonoBehaviour
{
    public Transform ball; // Ссылка на трансформ мяча
    public float hitPower = 10f; // Сила удара
    public float maxHitPower = 20f; // Максимальная сила удара
    public float hitPowerIncreaseRate = 2f; // Скорость увеличения силы удара

    private bool isAiming = false; // Флаг для определения, идет ли в данный момент прицеливание
    private Vector3 initialScale; // Исходный размер шкалы удара
    private float hitPowerScale = 1f; // Масштаб силы удара

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
                // Определение силы удара в зависимости от размера шкалы
                hitPower = maxHitPower * hitPowerScale;

                // Запуск мяча с использованием силы удара
                Vector3 force = transform.forward * hitPower;
                ball.GetComponent<Rigidbody>().AddForce(force, ForceMode.Impulse);

                // Сброс флага прицеливания и сброс размера шкалы
                isAiming = false;
                transform.localPosition = initialScale;
            }
        }
    }
}