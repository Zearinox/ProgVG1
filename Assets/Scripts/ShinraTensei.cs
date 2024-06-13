using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShinraTensei : MonoBehaviour
{
    public float pushForce = 10f;
    public float abilityRadius = 40f;
    public float abilityDuration = 1f;
    public float abilityCooldown = 5f;
    public float delayBeforeActivation = 0.5f; // Delay antes de que la habilidad se active
    public KeyCode abilityKey = KeyCode.P;
    public GameObject shockwaveEffect;
    public AudioClip abilitySound; // Sonido de la habilidad
    public float soundVolume = 1f;

    private bool isCooldown = false;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    void Update()
    {
        if (Input.GetKeyDown(abilityKey) && !isCooldown)
        {
            StartCoroutine(ActivateShinraTensei());
        }
    }

    private IEnumerator ActivateShinraTensei()
    {
        isCooldown = true;

        // Reproducir el sonido de la habilidad
        if (abilitySound != null)
        {
            audioSource.PlayOneShot(abilitySound, soundVolume);
        }

        // Esperar antes de activar la habilidad
        yield return new WaitForSeconds(delayBeforeActivation);

        // Mostrar el efecto visual de la onda expansiva
        if (shockwaveEffect != null)
        {
            GameObject effect = Instantiate(shockwaveEffect, transform.position, Quaternion.identity);
            ShockwaveController shockwaveController = effect.GetComponent<ShockwaveController>();
            if (shockwaveController != null)
            {
                shockwaveController.maxScale = abilityRadius;
            }

            Destroy(effect, abilityDuration + 1f);
        }

        float elapsedTime = 1f;
        float interval = 0.1f;

        while (elapsedTime < abilityDuration)
        {
            elapsedTime += interval;
            float currentRadius = (elapsedTime / abilityDuration) * abilityRadius;

            Collider[] colliders = Physics.OverlapSphere(transform.position, currentRadius);
            foreach (Collider nearbyObject in colliders)
            {
                Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    if (rb.isKinematic)
                    {
                        rb.isKinematic = false;
                    }

                    Vector3 direction = nearbyObject.transform.position - transform.position;
                    rb.AddForce(direction.normalized * pushForce, ForceMode.Impulse);
                }
            }

            yield return new WaitForSeconds(interval);
        }

        yield return new WaitForSeconds(abilityCooldown);
        isCooldown = false;
    }
}

