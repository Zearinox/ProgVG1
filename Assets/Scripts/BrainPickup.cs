using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrainPickup : MonoBehaviour
{
    public int points = 1; // Puntos que este objeto dará al jugador
    public AudioClip pickupSound; // El sonido que se reproducirá
    public GameObject efectoBrillo; // Referencia al objeto hijo EfectoBrillo

    private AudioSource audioSource;

    void Start()
    {
        
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                Debug.Log("Cerebro recogido, añadiendo puntos.");
                player.AddScore(points);

                // Reproducir el sonido inmediatamente
                if (pickupSound != null)
                {
                    // Reproducir el sonido y crear un objeto temporal para reproducirlo
                    AudioSource.PlayClipAtPoint(pickupSound, transform.position);

                    // Destruir los hijos inmediatamente
                    if (efectoBrillo != null)
                    {
                        Destroy(efectoBrillo);
                    }

                    // Destruir el objeto principal inmediatamente
                    Destroy(gameObject);
                }
                
            }
            
        }
        
    }
}