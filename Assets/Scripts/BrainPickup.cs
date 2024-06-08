using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrainPickup : MonoBehaviour
{
    public int points = 1; // Puntos que este objeto dará al jugador

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                Debug.Log("Cerebro recogido, añadiendo puntos.");
                player.AddScore(points);
                Destroy(gameObject); // Destruye el objeto después de recogerlo
            }
            else
            {
                Debug.LogWarning("El objeto con el que colisionó no tiene un componente PlayerController.");
            }
        }
        else
        {
            Debug.Log("El objeto con el que colisionó no tiene el tag 'Player'.");
        }
    }
}