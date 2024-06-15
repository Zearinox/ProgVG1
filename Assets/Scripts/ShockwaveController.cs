using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockwaveController : MonoBehaviour
{
    public float expansionDuration = 1f; // Duración de la expansión
    public float maxScale = 40f; // Escala máxima del efecto (radio de la habilidad)

    private Vector3 initialScale;

    void Start()
    {
        initialScale = transform.localScale;
        StartCoroutine(ExpandShockwave());
    }

    private IEnumerator ExpandShockwave()
    {
        float elapsedTime = 0f;

        while (elapsedTime < expansionDuration)
        {
            elapsedTime += Time.deltaTime;
            float scale = Mathf.Lerp(initialScale.x, maxScale, elapsedTime / expansionDuration);
            transform.localScale = new Vector3(scale, scale, scale);
            yield return null;
        }

        Destroy(gameObject); // Destruir el efecto después de la expansión
    }
}