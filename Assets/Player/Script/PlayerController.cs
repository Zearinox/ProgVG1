using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float runSpeed = 7;
    public float rotationSpeed = 10;
    public Animator animator;

    private float x, y;

    public Rigidbody rb;
    public float jumpForce = 5;
    public float rollForce = 5;
    public float evadeForce = 5;

    public Transform groundCheck;
    public float groundDistance = 0.1f;
    public LayerMask groundMask;

    private bool isGrounded;
    private bool canRoll = true;
    private bool canEvade = true;
    private bool isInvulnerable = false; // Variable para indicar si es invulnerable
    private float lastTapTime = 0f;
    private float doubleTapTime = 0.3f; // Tiempo permitido entre taps para rodar
    private bool awaitingSecondTap = false;

    public int maxHealth = 100;
    public int currentHealth;
    public Text healthText;
    public HealthBar healthBar; // Añade esta línea para la referencia a la barra de vida

    public int score = 0;
    public Text scoreText;
    public ScoreBar scoreBar; // Añadir referencia al ScoreBar


    private List<Renderer> renderers = new List<Renderer>();

    private void Start()
    {
        currentHealth = maxHealth;
        UpdateScoreUI();
        UpdateHealthUI();

        // Obtener los renderers del personaje
        renderers.AddRange(GetComponentsInChildren<Renderer>());
    }

    void Update()
    {
        if (currentHealth <= 0)
        {
            Die();
            return;
        }

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        x = Input.GetAxis("Horizontal");
        y = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(x, 0, y).normalized;

        transform.Rotate(0, x * Time.deltaTime * rotationSpeed, 0);
        transform.Translate(0, 0, y * Time.deltaTime * runSpeed);

        animator.SetFloat("VelX", x);
        animator.SetFloat("VelY", y);

        if (Input.GetKey("f"))
        {
            animator.Play("SnakeDance");
            animator.SetBool("Other", false);
        }
        if (Input.GetKey("g"))
        {
            animator.Play("BootyDance");
            animator.SetBool("Other", false);
        }

        if (x != 0 || y != 0)
        {
            animator.SetBool("Other", true);
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            if (awaitingSecondTap)
            {
                if (Time.time - lastTapTime < doubleTapTime)
                {
                    if (isGrounded && canRoll)
                    {
                        Roll();
                        StartCoroutine(RollCooldown());
                    }
                }
                awaitingSecondTap = false;
            }
            else
            {
                lastTapTime = Time.time;
                awaitingSecondTap = true;
                StartCoroutine(EvadeIfSingleTap());
            }
        }

        
    }

    private void Jump()
    {
        // Resetea la velocidad vertical antes de saltar
        rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);

        // Aplica la fuerza de salto
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

        // Reproduce la animación de salto
        animator.Play("Jump");
    }

    private void Roll()
    {
        StartCoroutine(TemporaryInvulnerability(animator.GetCurrentAnimatorStateInfo(0).length));
        animator.Play("Roll");
        rb.AddForce(transform.forward * rollForce, ForceMode.Impulse);
    }

    private void Evade()
    {
        // Detiene el movimiento
        rb.velocity = Vector3.zero;

        // Inicia la invulnerabilidad temporal durante la animación
        StartCoroutine(TemporaryInvulnerability(animator.GetCurrentAnimatorStateInfo(0).length));

        // Reproduce la animación de evasión
        animator.Play("Evade");

        // Aplica la fuerza de evasión
        rb.AddForce(-transform.forward * evadeForce, ForceMode.Impulse);
    }

    private IEnumerator RollCooldown()
    {
        canRoll = false;
        yield return new WaitForSeconds(2);
        canRoll = true;
    }

    private IEnumerator EvadeIfSingleTap()
    {
        yield return new WaitForSeconds(doubleTapTime);
        if (awaitingSecondTap && isGrounded && canEvade && !IsMoving())
        {
            Evade();
        }
        awaitingSecondTap = false;
    }

    private bool IsMoving()
    {
        return x != 0 || y != 0;
    }

    private IEnumerator TemporaryInvulnerability(float duration)
    {
        isInvulnerable = true;
        yield return new WaitForSeconds(duration);
        isInvulnerable = false;
    }

    public void TakeDamage(int damage)
    {
        if (isInvulnerable) return; // No recibe daño si es invulnerable

        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            StartCoroutine(DamageEffect());
            StartCoroutine(TemporaryInvulnerability(1f)); // Hacer invulnerable por 1 segundo después de recibir daño
        }
        UpdateHealthUI();
    }

    private IEnumerator DamageEffect()
    {
        for (int i = 0; i < 5; i++)
        {
            // Alternar la visibilidad del personaje
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

    private void Die()
    {
        animator.Play("Die");
    }

    public void AddScore(int points)
    {
        score += points;
        UpdateScoreUI();
        if (scoreBar != null)
        {
            scoreBar.UpdateScore(score); // Actualizar la barra de puntuación
        }
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Cerebros: " + score;
        }
    }

    private void UpdateHealthUI()
    {
        if (healthText != null)
        {
            healthText.text = "Vida: " + currentHealth;
        }
        if (healthBar != null)
        {
            healthBar.UpdateHealth(currentHealth);
        }
    }
}
