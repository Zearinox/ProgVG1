using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonballBehavior : MonoBehaviour
{
    public int directDamage = 1; // Daño directo que hace la bala de cañón
    public int explosionDamage = 1; // Daño por la explosión
    public float burnDuration = 10f; // Duración del área quemada
    public int burnDamagePerSecond = 1; // Daño por segundo en el área quemada
    public float explosionRadius = 5f; // Radio de la explosión
    public GameObject burnAreaPrefab; // Prefab del área quemada
    public GameObject explosionEffect; // Efecto de explosión visual
    public GameObject fireEffect; // Efecto visual de fuego quemado
    public AudioClip explosionSound; // Sonido de explosión
    public AudioClip fireSound; // Sonido de fuego
    public float maxDistance = 20f; // Distancia máxima para escuchar el sonido completamente

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

        // Destruir la bala de cañón
        Destroy(gameObject);
    }

    void Explode()
    {
        // Mostrar el efecto de explosión
        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, transform.position, transform.rotation);
        }

        // Reproducir el sonido de explosión con atenuación según la distancia
        if (explosionSound != null)
        {
            AudioSource explosionAudioSource = gameObject.AddComponent<AudioSource>();
            explosionAudioSource.clip = explosionSound;
            explosionAudioSource.spatialBlend = 1.0f; // Sonido 3D
            explosionAudioSource.maxDistance = maxDistance;
            explosionAudioSource.Play();
            Destroy(explosionAudioSource, explosionSound.length); // Destruir el AudioSource después de reproducir el sonido
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

        // Crear el área quemada
        if (burnAreaPrefab != null)
        {
            GameObject burnArea = Instantiate(burnAreaPrefab, transform.position, Quaternion.identity);
            BurnArea burnAreaScript = burnArea.GetComponent<BurnArea>();
            if (burnAreaScript != null)
            {
                burnAreaScript.Initialize(burnDuration, burnDamagePerSecond);
            }

            // Instanciar el efecto visual de fuego quemado y reproducir el sonido de fuego
            if (fireEffect != null)
            {
                GameObject fire = Instantiate(fireEffect, transform.position, Quaternion.identity);
                AudioSource fireAudioSource = fire.AddComponent<AudioSource>();
                fireAudioSource.clip = fireSound;
                fireAudioSource.spatialBlend = 1.0f; // Sonido 3D
                fireAudioSource.maxDistance = maxDistance;
                fireAudioSource.loop = true;
                fireAudioSource.Play();
                Destroy(fire, burnDuration); // Destruir el efecto visual después de la duración
            }
        }
    }
}