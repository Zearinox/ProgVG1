using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CrossbowBolts : MonoBehaviour
{
    [SerializeField] private int damage;
    [SerializeField] private float veloc;
    [SerializeField] private AudioClip impactPlayerSound; 
    [SerializeField] private AudioClip impactGroundSound; 
    [SerializeField] private AudioClip shootSound;
    private float impactVolume = 1f; 
    private float maxDistance = 20f;

    private void Start()
    {
        PlayImpactSound(shootSound);
    }

    void Update()
    {
        
        transform.Translate(0, 0, -veloc * Time.deltaTime);
        
       
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            
            PlayImpactSound(impactPlayerSound);

            
            PlayerController playerController = collision.gameObject.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.TakeDamage(damage); 
            }
        }
        else if (collision.gameObject.CompareTag("Enemy"))
        {
            
            PlayImpactSound(impactPlayerSound);

            
            EnemyAI enemyAI = collision.gameObject.GetComponent<EnemyAI>();
            if (enemyAI != null)
            {
                enemyAI.TakeDamage(damage); 
            }
        }
        else
        {
            
            PlayImpactSound(impactGroundSound);
        }

        
        Destroy(gameObject);
       

    }
    private void PlayImpactSound(AudioClip clip)
    {
        if (clip != null)
        {
            GameObject soundObject = new GameObject("ImpactSound");
            soundObject.transform.position = transform.position;
            AudioSource audioSource = soundObject.AddComponent<AudioSource>();
            audioSource.clip = clip;
            audioSource.spatialBlend = 1.0f; 
            audioSource.maxDistance = maxDistance;
            audioSource.volume = impactVolume;
            audioSource.Play();
            Destroy(soundObject, clip.length); 
        }
    }

}

