using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowTower : MonoBehaviour
{
    public GameObject arrowPrefab; // Prefab de la flecha
    public Transform firePoint; // Punto de origen de la flecha
    public float minFireRate = 1f; // Tiempo mínimo entre disparos
    public float maxFireRate = 3f; // Tiempo máximo entre disparos
    public float projectileSpeed = 10f; // Velocidad de la flecha
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
        target = GameObject.FindGameObjectWithTag("Player").transform;
        SetRandomFireCountdown();
        GameObject player = GameObject.FindGameObjectWithTag("Player"); //PARA QUE DEJE DE PEGAR
        if (player != null)
        {
            playerController = player.GetComponent<PlayerController>();
        }
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
            {if (playerController.currentHealth >= 0)
                {
                Shoot();
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

    void Shoot()
    {
        // Instanciar la flecha en el punto de origen
        GameObject arrowGO = Instantiate(arrowPrefab, firePoint.position, firePoint.rotation);
        ArrowBehavior arrowBehavior = arrowGO.GetComponent<ArrowBehavior>();
        if (arrowBehavior != null)
        {
            // Asignar la posición del objetivo al momento del disparo
            arrowBehavior.SetTargetPosition(target.position);
        }

        // Reproducir el sonido de disparo con atenuación según la distancia
        if (fireSound != null)
        {
            GameObject soundObject = new GameObject("FireSound");
            soundObject.transform.position = firePoint.position;
            AudioSource fireAudioSource = soundObject.AddComponent<AudioSource>();
            fireAudioSource.clip = fireSound;
            fireAudioSource.spatialBlend = 1.0f; // Sonido 3D
            fireAudioSource.maxDistance = maxDistance;
            fireAudioSource.volume = fireVolume;
            fireAudioSource.Play();
            Destroy(soundObject, fireSound.length); // Destruir el objeto de sonido después de reproducir el sonido
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