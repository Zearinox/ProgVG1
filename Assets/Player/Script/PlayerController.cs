using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float runSpeed = 7;
    public float rotationSpeed = 10; // Ajustado para una rotación más suave
    public Animator animator;

    private float x, y;
    private float turnSmoothVelocity;

    public Rigidbody rb;
    public float jumpForce = 5; // Fuerza de salto ajustada
    public float rollForce = 5; // Fuerza de rodar ajustada

    public Transform groundCheck;
    public float groundDistance = 0.1f;
    public LayerMask groundMask;

    private bool isGrounded;
    private bool canRoll = true;

    public int maxHealth = 10;
    public int currentHealth;

    private void Start()
    {
        currentHealth = maxHealth;
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

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, 0.1f);
            transform.rotation = Quaternion.Euler(0, angle, 0);

            Vector3 moveDir = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;
            rb.MovePosition(transform.position + moveDir.normalized * runSpeed * Time.deltaTime);
        }

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

        if (Input.GetKeyDown(KeyCode.Z) && isGrounded && canRoll)
        {
            Roll();
            StartCoroutine(RollCooldown());
        }
    }

    private void Jump()
    {
        // Reproduce la animación de salto
        animator.Play("Jump");

        // Resetea la velocidad vertical antes de saltar
        rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);

        // Aplica la fuerza de salto
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    private void Roll()
    {
        animator.Play("Roll");
        rb.AddForce(transform.forward * rollForce, ForceMode.Impulse);
    }

    private IEnumerator RollCooldown()
    {
        canRoll = false;
        yield return new WaitForSeconds(2);
        canRoll = true;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        animator.Play("Die");
    }
}