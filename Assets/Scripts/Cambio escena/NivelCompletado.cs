using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NivelCompletado : MonoBehaviour
{
    public Text pressAnyKeyText; // Referencia al texto de "Presiona cualquier tecla"
    public float blinkDuration = 0.5f; // Duración del parpadeo en segundos
    private bool blinkState = true;
    public AudioClip pressKeySound;
    public float pressVolume = 1f;
    private static int previousSceneIndex;

    void Start()
    {
        // Obtener el índice de la escena anterior
        previousSceneIndex = PlayerPrefs.GetInt("PreviousSceneIndex");

        if (pressAnyKeyText != null)
        {
            StartCoroutine(BlinkText());
        }
        else
        {
            Debug.LogError("No se ha asignado el texto para 'Presiona cualquier tecla'.");
        }
    }

    void Update()
    {
        // Detectar si cualquier tecla es presionada
        if (Input.anyKeyDown)
        {
            pressStart();
            // Cargar la escena del primer nivel (asegúrate de que el nombre de la escena sea correcto)

        }
    }

    private void pressStart()
    {
        if (pressKeySound != null)
        {
            AudioSource pressKeyAudioSource = gameObject.AddComponent<AudioSource>();
            pressKeyAudioSource.clip = pressKeySound;
            pressKeyAudioSource.spatialBlend = 0.0f;
            pressKeyAudioSource.loop = false;
            pressKeyAudioSource.volume = pressVolume;
            pressKeyAudioSource.Play();
            Destroy(pressKeyAudioSource, pressKeySound.length);
        }
        StartCoroutine(pressStartDelay(3f));
    }

    IEnumerator BlinkText()
    {
        while (true)
        {
            blinkState = !blinkState;
            pressAnyKeyText.enabled = blinkState;
            yield return new WaitForSeconds(blinkDuration);
        }
    }

    IEnumerator pressStartDelay(float delay)
    {
        // Calcular el índice de la siguiente escena
        int nextSceneIndex = previousSceneIndex + 1;

        // Verificar si la siguiente escena existe en el Build Settings
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            yield return new WaitForSeconds(delay);
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            Debug.LogWarning("No hay más escenas disponibles para cargar.");
            // Aquí puedes manejar qué sucede si no hay más escenas disponibles (por ejemplo, reiniciar el juego).
        }
    }
}
