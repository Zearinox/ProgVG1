using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonballBehavior : MonoBehaviour
{
    public int directDamage = 1;
    public int explosionDamage = 1;
    public float burnDuration = 10f;
    public int burnDamagePerSecond = 1;
    public float explosionRadius = 5f;
    public GameObject burnAreaPrefab;
    public GameObject explosionEffect;
    public GameObject fireEffect;
    public AudioClip explosionSound;
    public AudioClip fireSound;
    public float maxDistance = 20f;
    public float explosionVolume = 1f;
    public float fireVolume = 1f;
    public float explosionDuration = 2f;

    private void OnCollisionEnter(Collision collision)
    {
        Explode();

        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController playerController = collision.gameObject.GetComponent<PlayerController>();
            if (playerController != null)
            {
                Vector3 directionToPlayer = collision.transform.position - transform.position;
                RaycastHit hit;
                if (!Physics.Raycast(transform.position, directionToPlayer, out hit, explosionRadius))
                {
                    playerController.TakeDamage(directDamage);
                }
            }
        }
    }

    void Explode()
    {
        if (explosionEffect != null)
        {
            GameObject explosion = Instantiate(explosionEffect, transform.position, transform.rotation);
            Destroy(explosion, explosionDuration);
        }

        if (explosionSound != null)
        {
            AudioSource explosionAudioSource = gameObject.AddComponent<AudioSource>();
            explosionAudioSource.clip = explosionSound;
            explosionAudioSource.spatialBlend = 1.0f;
            explosionAudioSource.maxDistance = maxDistance;
            explosionAudioSource.volume = explosionVolume;
            explosionAudioSource.Play();
            Destroy(explosionAudioSource, explosionSound.length);
        }

        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider nearbyObject in colliders)
        {
            if (nearbyObject.CompareTag("Player"))
            {
                PlayerController playerController = nearbyObject.GetComponent<PlayerController>();
                if (playerController != null)
                {
                    Vector3 directionToPlayer = nearbyObject.transform.position - transform.position;
                    RaycastHit hit;
                    if (!Physics.Raycast(transform.position, directionToPlayer, out hit, explosionRadius))
                    {
                        playerController.TakeDamage(explosionDamage);
                    }
                }
            }
        }

        if (burnAreaPrefab != null)
        {
            GameObject burnArea = Instantiate(burnAreaPrefab, transform.position, Quaternion.identity);
            BurnArea burnAreaScript = burnArea.GetComponent<BurnArea>();
            if (burnAreaScript != null)
            {
                burnAreaScript.Initialize(burnDuration, burnDamagePerSecond);
            }

            if (fireEffect != null)
            {
                GameObject fire = Instantiate(fireEffect, transform.position, Quaternion.identity);
                AudioSource fireAudioSource = fire.AddComponent<AudioSource>();
                fireAudioSource.clip = fireSound;
                fireAudioSource.spatialBlend = 1.0f;
                fireAudioSource.maxDistance = maxDistance;
                fireAudioSource.loop = true;
                fireAudioSource.volume = fireVolume;
                fireAudioSource.Play();
                Destroy(fire, burnDuration);
            }
        }

        Destroy(gameObject);
    }
}
