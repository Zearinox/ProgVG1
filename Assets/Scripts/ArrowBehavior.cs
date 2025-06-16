using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowBehavior : MonoBehaviour
{
    public int damage = 1; // Daño directo que hace la flecha
    public float maxLifetime = 5f; // Tiempo máximo antes de que la flecha desaparezca
    public float speed = 10f; // Velocidad de la flecha
    public AudioClip impactPlayerSound; // Sonido al impactar contra el jugador
    public AudioClip impactGroundSound; // Sonido al impactar contra el terreno u otros objetos sólidos
    public float impactVolume = 1f; // Volumen del sonido de impacto
    public float maxDistance = 20f; // Distancia máxima para escuchar el sonido completamente

    private Vector3 targetPosition; // Posición del objetivo en el momento del disparo
    

    void Start()
    {
        // Destruir la flecha después de un tiempo
        Destroy(gameObject, maxLifetime);
    }

    public void SetTargetPosition(Vector3 position)
    {
        targetPosition = position;
        
        // Calcular la dirección inicial hacia el objetivo
        Vector3 direction = (targetPosition - transform.position).normalized;
        GetComponent<Rigidbody>().linearVelocity = direction * speed;
        transform.rotation = Quaternion.LookRotation(direction);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Determinar el tipo de colisión y reproducir el sonido adecuado
        if (collision.gameObject.CompareTag("Player"))
        {
            // Reproducir el sonido de impacto contra el jugador
            PlayImpactSound(impactPlayerSound);

            // Infligir daño al jugador
            PlayerController playerController = collision.gameObject.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.TakeDamage(damage); // Hacer daño directo al jugador
            }
        }
        else if (collision.gameObject.CompareTag("Enemy"))
        {
            // Reproducir el sonido de impacto contra el enemigo (puede ser el mismo que el del jugador)
            PlayImpactSound(impactPlayerSound);

            // Infligir daño al enemigo
            EnemyAI enemyAI = collision.gameObject.GetComponent<EnemyAI>();
            if (enemyAI != null)
            {
                enemyAI.TakeDamage(damage); // Hacer daño directo al enemigo
            }
        }
        else
        {
            // Reproducir el sonido de impacto contra el terreno u otros objetos sólidos
            PlayImpactSound(impactGroundSound);
        }

        // Destruir la flecha al colisionar con cualquier cosa
        Destroy(gameObject);
    }

    private void PlayImpactSound(AudioClip clip)
    {
        if (clip != null)
        {
            GameObject soundObject = new GameObject("ImpactSound");
            soundObject.transform.position = transform.position;
            AudioSource audioSource = soundObject.AddComponent<AudioSource>();
            audioSource.clip = clip;
            audioSource.spatialBlend = 1.0f; // Sonido 3D
            audioSource.maxDistance = maxDistance;
            audioSource.volume = impactVolume;
            audioSource.Play();
            Destroy(soundObject, clip.length); // Destruir el objeto de sonido después de reproducir el sonido
        }
    }
}