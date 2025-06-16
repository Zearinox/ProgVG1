using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonTower111 : MonoBehaviour
{
    public GameObject projectilePrefab; // Prefab del proyectil
    public Transform firePoint; // Punto de origen del proyectil
    public float minFireRate = 1f; // Tiempo mínimo entre disparos
    public float maxFireRate = 3f; // Tiempo máximo entre disparos
    public float projectileSpeed = 10f; // Velocidad del proyectil
    public float detectionRange = 50f; // Rango de detección del objetivo
    public float minDetectionRange = 15f;
    public Vector3 rotationOffset; // Offset de rotación para ajustar la orientación
    
    
    private Transform target; // Referencia al objetivo (personaje)
    private float fireCountdown;

    void Start()
    {
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

            // Rotar el cañón para que mire al objetivo con el offset aplicado
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            lookRotation *= Quaternion.Euler(rotationOffset);
            transform.rotation = lookRotation;

            // Disparar proyectiles a intervalos aleatorios
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
        // Instanciar el proyectil en el punto de origen
        GameObject projectileGO = Instantiate(projectilePrefab, firePoint.position, Quaternion.LookRotation(direction));
        Rigidbody rb = projectileGO.GetComponent<Rigidbody>();
        if (rb != null)
        {
            // Aplicar fuerza para disparar el proyectil
            rb.linearVelocity = direction * projectileSpeed;
        }
    }

    void OnDrawGizmosSelected()
    {
        // Dibujar el rango de detección en la vista de la escena
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}