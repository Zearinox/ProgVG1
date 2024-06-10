using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class VictoryScene : MonoBehaviour
{
    public Text pressAnyKeyText; // Referencia al texto de "Presiona cualquier tecla"
    public float blinkDuration = 0.5f; // Duración del parpadeo en segundos
    private bool blinkState = true;
    public AudioClip pressKeySound;
    public float pressVolume = 1f;

    void Start()
    {
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
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene("Main Menu");
    }
}