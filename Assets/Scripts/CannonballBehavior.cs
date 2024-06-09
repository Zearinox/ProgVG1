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
        }
    }
}