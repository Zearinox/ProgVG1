using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowBehavior : MonoBehaviour
{
    public int damage = 1; // Daño que hará la flecha

    private void OnCollisionEnter(Collision collision)
    {
        // Si colisiona con el jugador
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController playerController = collision.gameObject.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.TakeDamage(damage); // Hacer daño al jugador
            }
            Destroy(gameObject); // Destruir la flecha después del impacto
        }
        else // Si colisiona con cualquier otro objeto
        {
            Destroy(gameObject, 3f); // Destruir la flecha después de 3 segundos
        }
    }
}