using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
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
        // Salir del juego al presionar "Esc"
        if (Input.GetKeyDown(KeyCode.Escape))        
            Application.Quit();        

        // Iniciar nvl 1 al presionar "Enter"
        if (Input.anyKeyDown)        
            pressStart();                   
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
        SceneManager.LoadScene("Selecction");
    }
}