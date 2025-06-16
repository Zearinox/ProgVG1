using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonTower : MonoBehaviour
{
    public GameObject cannonballPrefab; // Prefab de la bala de cañón
    public Transform firePoint; // Punto de origen de la bala de cañón
    public float minFireRate = 1f; // Tiempo mínimo entre disparos
    public float maxFireRate = 3f; // Tiempo máximo entre disparos
    public float projectileSpeed = 10f; // Velocidad de la bala de cañón
    public float detectionRange = 15f; // Rango máximo de detección del objetivo
    public float minDetectionRange = 5f; // Rango mínimo de detección del objetivo
    public Vector3 rotationOffset; // Offset de rotación para ajustar la orientación
    public AudioClip fireSound; // Sonido al disparar
    public float maxDistance = 20f; // Distancia máxima para escuchar el sonido completamente
    public float fireVolume = 1f; // Volumen del sonido de disparo
    private PlayerController playerController;

    private Transform target; // Referencia al objetivo (personaje)
    private float fireCountdown;
    


    void Start()
    {
        // Encuentra al jugador por etiqueta
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerController = player.GetComponent<PlayerController>();
        }
        target = GameObject.FindGameObjectWithTag("Player").transform;
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
                if (playerController.currentHealth >= 0)
                {
                Shoot(direction);
                SetRandomFireCountdown();
                }
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
        GameObject cannonballGO = Instantiate(cannonballPrefab, firePoint.position, Quaternion.LookRotation(direction));
        Rigidbody rb = cannonballGO.GetComponent<Rigidbody>();
        if (rb != null)
        {
            // Aplicar fuerza para disparar el proyectil
            rb.linearVelocity = direction * projectileSpeed;
        }

        // Reproducir el sonido de disparo con atenuación según la distancia
        if (fireSound != null)
        {
            AudioSource fireAudioSource = gameObject.AddComponent<AudioSource>();
            fireAudioSource.clip = fireSound;
            fireAudioSource.spatialBlend = 1.0f; // Sonido 3D
            fireAudioSource.maxDistance = maxDistance;
            fireAudioSource.volume = fireVolume;
            fireAudioSource.Play();
            Destroy(fireAudioSource, fireSound.length); // Destruir el AudioSource después de reproducir el sonido
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