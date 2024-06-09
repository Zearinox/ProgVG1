using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Image healthBarImage;
    public GameObject leftEdge;
    public GameObject rightEdge;

    private float maxHealth = 100f;
    private float currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthBar();
    }

    public void UpdateHealth(float health)
    {
        currentHealth = health;
        UpdateHealthBar();
    }

    private void UpdateHealthBar()
    {
        healthBarImage.fillAmount = currentHealth / maxHealth;

        // Controlar la visibilidad de los bordes
        if (currentHealth <= 0)
        {
            leftEdge.SetActive(false);
        }
        else
        {
            leftEdge.SetActive(true);
            
        }
        if (currentHealth < maxHealth)
        {
            rightEdge.SetActive(false);
        }
        else
        {
            rightEdge.SetActive(true);
        }
        
    }
}