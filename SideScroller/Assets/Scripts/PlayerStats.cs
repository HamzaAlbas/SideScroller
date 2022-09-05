using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] protected float health;
    [SerializeField] protected float maxHealth;

    [SerializeField] protected bool isDead;

    [SerializeField] private Animator animator;

    private void Start()
    {
        InitVariables();
        HealthbarController.Instance.SetHealthBar(health, maxHealth);
    }

    private void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            TakeDamage(10);
        }
    }

    public void CheckHealth()
    {
        if (health <= 0)
        {
            health = 0;
            HealthbarController.Instance.SetHealthBar(health, maxHealth);

            Die();
        }
        if (health >= maxHealth)
        {
            health = maxHealth;
            HealthbarController.Instance.SetHealthBar(health, maxHealth);

        }
    }

    public void SetHealth(float newHealth)
    {
        health = newHealth;
        HealthbarController.Instance.SetHealthBar(health, maxHealth);
        CheckHealth();
    }

    public void Die()
    {
        isDead = true;
        animator.SetTrigger("PlayerDead");
        Debug.Log("Dead");
    }

    public void TakeDamage(int damage)
    {
        float newHealth = health - damage;
        SetHealth(newHealth);
        CheckHealth();
    }

    public void Heal(int heal)
    {
        float newHealth = health + heal;
        SetHealth(newHealth);
        CheckHealth();
    }

    public void InitVariables()
    {
        SetHealth(maxHealth);
    }
}
