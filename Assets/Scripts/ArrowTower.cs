using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowTower : MonoBehaviour
{
    public GameObject arrowPrefab; // Prefab de la flecha
    public Transform firePoint; // Punto de origen de la flecha
    public float minFireRate = 1f; // Tiempo mínimo entre disparos
    public float maxFireRate = 3f; // Tiempo máximo entre disparos
    public float arrowSpeed = 10f; // Velocidad de la flecha
    public float detectionRange = 15f; // Rango máximo de detección del objetivo
    public float minDetectionRange = 5f; // Rango mínimo de detección del objetivo
    public Vector3 rotationOffset; // Offset de rotación para ajustar la orientación
    public Transform target; // Referencia al objetivo (personaje)
    private float fireCountdown;

    void Start()
    {
        // Encuentra al jugador por etiqueta
        SetRandomFireCountdown();
    }

    void Update()
    {
        if (target == null)
            return;

        // Verificar si el objetivo está dentro del rango de detección adecuado
        float distanceToTarget = Vector3.Distance(transform.position, target.position);
        if (distanceToTarget <= detectionRange && distanceToTarget >= minDetectionRange)
        {
            // Calcular dirección hacia el objetivo
            Vector3 direction = (target.position - transform.position).normalized;

            // Rotar la torre para que mire al objetivo con el offset aplicado
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            lookRotation *= Quaternion.Euler(rotationOffset);
            transform.rotation = lookRotation;

            // Disparar flechas a intervalos aleatorios
            if (fireCountdown <= 0f)
            {
                Shoot(direction);
                SetRandomFireCountdown();
            }

            fireCountdown -= Time.deltaTime;
        }
    }

    void SetRandomFireCountdown()
    {
        fireCountdown = Random.Range(minFireRate, maxFireRate);
    }

    void Shoot(Vector3 direction)
    {
        // Instanciar la flecha en el punto de origen
        GameObject arrowGO = Instantiate(arrowPrefab, firePoint.position, Quaternion.LookRotation(direction));
        Rigidbody rb = arrowGO.GetComponent<Rigidbody>();
        if (rb != null)
        {
            // Aplicar fuerza para disparar la flecha
            rb.velocity = direction * arrowSpeed;
        }
    }

    void OnDrawGizmosSelected()
    {
        // Dibujar el rango de detección en la vista de la escena
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, minDetectionRange);
    }
}