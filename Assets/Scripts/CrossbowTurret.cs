using UnityEngine;
using System.Collections;

public class CrossbowTurret : MonoBehaviour
{
    public GameObject arrowPrefab; // Prefab de la flecha
    public Transform playerTransform; // Referencia al transform del jugador
    public float minFireRate = 2f; // Intervalo mínimo de disparo
    public float maxFireRate = 4f; // Intervalo máximo de disparo

    private void Start()
    {
        StartCoroutine(FireArrows());
    }

    private IEnumerator FireArrows()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minFireRate, maxFireRate)); // Espera un tiempo aleatorio

            if (playerTransform != null)
            {
                GameObject arrow = Instantiate(arrowPrefab, transform.position, Quaternion.identity); // Instancia una nueva flecha
                arrow.GetComponent<Rigidbody>().velocity = (playerTransform.position - transform.position).normalized * 10f; // Establece la velocidad de la flecha hacia el jugador
            }
        }
    }
}