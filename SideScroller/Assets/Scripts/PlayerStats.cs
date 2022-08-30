using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] protected int health;
    [SerializeField] protected int maxHealth;

    [SerializeField] protected bool isDead;

    private void Start()
    {
        InitVariables();
    }

    public void CheckHealth()
    {
        if(health <= 0)
        {
            health = 0;
            Die();
        }
        if(health <= maxHealth)
        {
            health = maxHealth;
        }
    }

    public void Die()
    {
        isDead = true;
    }

    public void SetHealth(int newHealth)
    {
        health = newHealth;
        CheckHealth();
    }

    public void TakeDamage(int damage)
    {
        int healthAfterDamage = health - damage;
        SetHealth(healthAfterDamage);
    }

    public void Heal(int heal)
    {
        int healthAfterHeal = health + heal;
        SetHealth(healthAfterHeal);
    }

    public void InitVariables()
    {
        maxHealth = 100;
        SetHealth(maxHealth);
        isDead = false;
    }
}
