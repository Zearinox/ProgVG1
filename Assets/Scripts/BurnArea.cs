using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurnArea : MonoBehaviour
{
    private float duration;
    private int damagePerSecond;

    public void Initialize(float duration, int damagePerSecond)
    {
        this.duration = duration;
        this.damagePerSecond = damagePerSecond;
        StartCoroutine(DealDamageOverTime());
        Destroy(gameObject, duration); // Destruir el área quemada después de la duración
    }

    private IEnumerator DealDamageOverTime()
    {
        while (true)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, GetComponent<SphereCollider>().radius);
            foreach (Collider nearbyObject in colliders)
            {
                if (nearbyObject.CompareTag("Player"))
                {
                    PlayerController playerController = nearbyObject.GetComponent<PlayerController>();
                    if (playerController != null)
                    {
                        playerController.TakeDamage(damagePerSecond); // Hacer daño por segundo al jugador
                    }
                }
            }
            yield return new WaitForSeconds(1f);
        }
    }
}