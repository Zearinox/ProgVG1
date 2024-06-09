using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingProjectile : MonoBehaviour
{
    public float speed = 10f; // Velocidad del proyectil
    public float homingDuration = 5f; // Duración durante la cual el proyectil persigue al objetivo
    public int directDamage = 1; // Daño directo que hace el proyectil
    public int explosionDamage = 1; // Daño por la explosión
    public float explosionRadius = 5f; // Radio de la explosión
    public GameObject explosionEffect; // Efecto de explosión visual
    public AudioClip explosionSound; // Sonido de explosión
    public float explosionVolume = 1f; // Volumen de la explosión
    public float maxDistance = 20f; // Distancia máxima para escuchar el sonido completamente

    private Transform target; // Referencia al objetivo (jugador)
    private float homingStartTime;

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        homingStartTime = Time.time;
        StartCoroutine(ExplodeAfterTime(homingDuration)); // Iniciar la corrutina para explotar después del tiempo definido
    }

    void Update()
    {
        if (target != null && Time.time - homingStartTime <= homingDuration)
        {
            // Perseguir al objetivo
            Vector3 direction = (target.position - transform.position).normalized;
            transform.position += direction * speed * Time.deltaTime;
            transform.LookAt(target);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Generar la explosión al colisionar con cualquier cosa
        Explode();

        // Si colisiona con el jugador
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController playerController = collision.gameObject.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.TakeDamage(directDamage); // Hacer daño directo al jugador
            }
        }

        // Destruir el proyectil
        Destroy(gameObject);
    }

    private IEnumerator ExplodeAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        Explode();
    }

    void Explode()
    {
        // Mostrar el efecto de explosión
        if (explosionEffect != null)
        {
            GameObject explosion = Instantiate(explosionEffect, transform.position, transform.rotation);
            Destroy(explosion, 2f); // Destruir la animación de explosión después de la duración
        }

        // Reproducir el sonido de explosión con atenuación según la distancia
        if (explosionSound != null)
        {
            // Crear un objeto vacío para reproducir el sonido
            GameObject soundObject = new GameObject("ExplosionSound");
            soundObject.transform.position = transform.position;
            AudioSource audioSource = soundObject.AddComponent<AudioSource>();
            audioSource.clip = explosionSound;
            audioSource.spatialBlend = 1.0f; // Sonido 3D
            audioSource.maxDistance = maxDistance;
            audioSource.volume = explosionVolume;
            audioSource.Play();
            Destroy(soundObject, explosionSound.length); // Destruir el objeto de sonido después de reproducir el sonido
        }

        // Encontrar todos los objetos en el radio de la explosión
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider nearbyObject in colliders)
        {
            // Infligir daño por la explosión al jugador
            if (nearbyObject.CompareTag("Player"))
            {
                PlayerController playerController = nearbyObject.GetComponent<PlayerController>();
                if (playerController != null)
                {
                    playerController.TakeDamage(explosionDamage); // Hacer daño por la explosión al jugador
                }
            }
        }

        // Destruir el proyectil
        Destroy(gameObject);
    }
}