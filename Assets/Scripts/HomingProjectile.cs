using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingProjectile : MonoBehaviour
{
    public float speed = 10f;
    public float homingDuration = 5f;
    public int directDamage = 1;
    public int explosionDamage = 1;
    public float explosionRadius = 5f;
    public GameObject explosionEffect;
    public AudioClip explosionSound;
    public float explosionVolume = 1f;
    public float maxDistance = 20f;

    private Transform target;
    private float homingStartTime;

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        homingStartTime = Time.time;
        StartCoroutine(ExplodeAfterTime(homingDuration));
    }

    void Update()
    {
        if (target != null && Time.time - homingStartTime <= homingDuration)
        {
            Vector3 direction = (target.position - transform.position).normalized;
            transform.position += direction * speed * Time.deltaTime;
            transform.LookAt(target);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController playerController = collision.gameObject.GetComponent<PlayerController>();
            if (playerController != null)
            {
                Vector3 directionToPlayer = collision.transform.position - transform.position;
                RaycastHit hit;
                if (Physics.Raycast(transform.position, directionToPlayer, out hit))
                {
                    if (hit.collider.CompareTag("Player"))
                    {
                        playerController.TakeDamage(directDamage);
                    }
                }
            }
        }
        
        Explode();
        Destroy(gameObject);
    }

    private IEnumerator ExplodeAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        Explode();
    }

    void Explode()
    {
        if (explosionEffect != null)
        {
            GameObject explosion = Instantiate(explosionEffect, transform.position, transform.rotation);
            Destroy(explosion, 2f);
        }

        if (explosionSound != null)
        {
            GameObject soundObject = new GameObject("ExplosionSound");
            soundObject.transform.position = transform.position;
            AudioSource audioSource = soundObject.AddComponent<AudioSource>();
            audioSource.clip = explosionSound;
            audioSource.spatialBlend = 1.0f;
            audioSource.maxDistance = maxDistance;
            audioSource.volume = explosionVolume;
            audioSource.Play();
            Destroy(soundObject, explosionSound.length);
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
                    if (Physics.Raycast(transform.position, directionToPlayer, out hit))
                    {
                        if (hit.collider.CompareTag("Player"))
                        {
                            playerController.TakeDamage(explosionDamage);
                        }
                    }
                }
            }
        }
    }
}
