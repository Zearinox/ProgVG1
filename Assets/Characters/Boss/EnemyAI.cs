using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EnemyAI : MonoBehaviour
{
    public Transform player; // Referencia al jugador
    public float detectionRadius = 10f; // Radio de detección
    
    [SerializeField] private float melee;
    public int maxHealth = 300; // Puntos de vida del enemigo
    public int currentHealth; // Vida actual del enemigo
    public int damage = 15; // Daño del ataque cuerpo a cuerpo
    public float attackCooldown = 1.5f; // Tiempo de espera entre ataques
    public Animator animator; // Referencia al componente Animator del enemigo
    public Vector3 rotationOffset; // Offset de rotación para ajustar la orientación
    public AudioClip painDeathSound; // Sonido de dolor/muerte
    public float deathVolume = 1f; // Volumen del sonido de muerte

    private NavMeshAgent navMeshAgent;
    private bool playerDetected = false;
    private float lastAttackTime = 0;
    private bool isDead = false; // Variable para verificar si el enemigo ya está muerto
    private bool isInvulnerable = false; // Variable para indicar si es invulnerable
    private List<Renderer> renderers = new List<Renderer>();
    private PlayerController playerController;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        currentHealth = maxHealth;
        renderers.AddRange(GetComponentsInChildren<Renderer>());
        GameObject player = GameObject.FindGameObjectWithTag("Player"); //PARA QUE DEJE DE PEGAR
        if (player != null)
        {
            playerController = player.GetComponent<PlayerController>();
        }
    }

    void Update()
    {
        if (Input.GetKey("t"))
        {
            animator.Play("Die");
            Die();
        }
        if (isDead) return; // Si el enemigo está muerto, no ejecutar el resto del código

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        
        if (playerController.currentHealth >= 0)
        {
            if (distanceToPlayer <= detectionRadius)
            {
                playerDetected = true;
                navMeshAgent.SetDestination(player.position); // Usar NavMesh para seguir al jugador
                animator.SetBool("isWalking", true); // Activar animación de caminar
                
            }
            else
            {
                playerDetected = false;
                navMeshAgent.SetDestination(transform.position); // Detener el movimiento del enemigo
                animator.SetBool("isWalking", false); // Desactivar animación de caminar
            }

            if (playerDetected && distanceToPlayer <= melee && Time.time > lastAttackTime + attackCooldown)
            {
                
                AttackPlayer();
                
            }
        }
        else
        {
            animator.SetBool("isWalking", false); 
        }
    }

    void AttackPlayer()
    {
        // Ejecutar animación de ataque
        
        animator.Play("Attack");

        // Implementar lógica de daño aquí
        PlayerController playerController = player.GetComponent<PlayerController>();
        if (playerController != null)
        {
            playerController.TakeDamage(damage);
        }
        lastAttackTime = Time.time;
    }

    public void TakeDamage(int damage)
    {
        if (isInvulnerable) return;

        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            StartCoroutine(DamageEffect());
            StartCoroutine(TemporaryInvulnerability(1f));
        }
    }

    private IEnumerator DamageEffect()
    {
        for (int i = 0; i < 5; i++)
        {
            foreach (var renderer in renderers)
            {
                renderer.enabled = false;
            }
            yield return new WaitForSeconds(0.1f);
            foreach (var renderer in renderers)
            {
                renderer.enabled = true;
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    private IEnumerator TemporaryInvulnerability(float duration)
    {
        isInvulnerable = true;
        yield return new WaitForSeconds(duration);
        isInvulnerable = false;
    }

    private void Die()
    {
        if (!isDead)
        {
            isDead = true;
            // Desactivar el NavMeshAgent para detener el movimiento
            navMeshAgent.isStopped = true;
            navMeshAgent.enabled = false;
            animator.Play("Die");
            if (painDeathSound != null)
            {
                AudioSource painDeathAudioSource = gameObject.AddComponent<AudioSource>();
                painDeathAudioSource.clip = painDeathSound;
                painDeathAudioSource.spatialBlend = 0.0f;
                painDeathAudioSource.volume = deathVolume;
                painDeathAudioSource.loop = false; // Asegúrate de que no se repita
                painDeathAudioSource.Play();
                Destroy(painDeathAudioSource, painDeathSound.length); // Destruye el AudioSource después de que se complete la reproducción
            }
            StartCoroutine(HandleDeath(4f));
        }
    }

    private IEnumerator HandleDeath(float delay)
    {
        // Esperar hasta que termine la animación de muerte
        yield return new WaitForSeconds(delay);

        // Cambiar a la escena de victoria
        SceneManager.LoadScene("VictoryScene");
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, melee);
    }
}
