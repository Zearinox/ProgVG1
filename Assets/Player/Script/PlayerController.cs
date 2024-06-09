using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float runSpeed = 7;
    public float rotationSpeed = 10;
    public Animator animator;

    private float x, y;
    private float turnSmoothVelocity;
    private bool isLockedOn = false;
    private Transform lockOnTarget;

    public Rigidbody rb;
    public float jumpForce = 5;
    public float rollForce = 5;

    public Transform groundCheck;
    public float groundDistance = 0.1f;
    public LayerMask groundMask;

    private bool isGrounded;
    private bool canRoll = true;
    private bool isRolling = false;

    public int maxHealth = 10;
    public int currentHealth;

    public int score = 0;
    public Text scoreText;
    public Text healthText;

    private void Start()
    {
        currentHealth = maxHealth;
        UpdateScoreUI();
        UpdateHealthUI();
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

        HandleMovement();
        HandleRotation();

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
        }

        if (Input.GetKeyDown(KeyCode.Z) && isGrounded && canRoll)
        {
            Roll();
            StartCoroutine(RollCooldown());
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            ToggleLockOn();
        }
    }

    void FixedUpdate()
    {
        if (x != 0 || y != 0)
        {
            Vector3 direction = new Vector3(x, 0, y).normalized;
            Vector3 moveDir = Quaternion.Euler(0, Camera.main.transform.eulerAngles.y, 0) * direction;
            rb.MovePosition(transform.position + moveDir.normalized * runSpeed * Time.fixedDeltaTime);
        }
    }

    private void HandleMovement()
    {
        animator.SetFloat("VelX", x);
        animator.SetFloat("VelY", y);

        if (x != 0 || y != 0)
        {
            animator.SetBool("Other", true);
        }
        else
        {
            animator.SetBool("Other", false);
        }
    }

    private void HandleRotation()
    {
        if (isLockedOn && lockOnTarget != null)
        {
            Vector3 targetDir = lockOnTarget.position - transform.position;
            targetDir.y = 0;
            transform.rotation = Quaternion.LookRotation(targetDir);
        }
        else
        {
            Vector3 direction = new Vector3(x, 0, y).normalized;
            if (direction.magnitude >= 0.1f)
            {
                float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, 0.1f);
                transform.rotation = Quaternion.Euler(0, angle, 0);
            }
        }
    }

    private void Jump()
    {
        animator.Play("Jump");
        rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    private void Roll()
    {
        animator.Play("Roll");
        rb.AddForce(transform.forward * rollForce, ForceMode.Impulse);
        isRolling = true;
        StartCoroutine(DisableRollingAfterAnimation());
    }

    private IEnumerator RollCooldown()
    {
        canRoll = false;
        yield return new WaitForSeconds(2);
        canRoll = true;
    }

    private IEnumerator DisableRollingAfterAnimation()
    {
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        isRolling = false;
    }

    public void TakeDamage(int damage)
    {
        if (isRolling)
        {
            return;
        }

        currentHealth -= damage;
        UpdateHealthUI();

        if (currentHealth <= 0)
        {
            Die();
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
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score;
        }
    }

    private void UpdateHealthUI()
    {
        if (healthText != null)
        {
            healthText.text = "Health: " + currentHealth;
        }
    }

    private void ToggleLockOn()
    {
        if (isLockedOn)
        {
            isLockedOn = false;
            lockOnTarget = null;
        }
        else
        {
            FindLockOnTarget();
        }
    }

    private void FindLockOnTarget()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 10f);
        float closestDistance = Mathf.Infinity;
        Transform closestTarget = null;

        foreach (Collider hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Enemy"))
            {
                float distance = Vector3.Distance(transform.position, hitCollider.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestTarget = hitCollider.transform;
                }
            }
        }

        if (closestTarget != null)
        {
            lockOnTarget = closestTarget;
            isLockedOn = true;
        }
    }
}