using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieStats : MonoBehaviour
{
    public static ZombieStats Instance { get; set; }
    [SerializeField] protected float health;
    [SerializeField] protected float maxHealth;
    [SerializeField] protected bool isDead;
    [SerializeField] private Animator animator;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        InitVariables();
        ZombieHealthbarController.Instance.SetHealthBar(health, maxHealth);
    }

    private void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            TakeDamage(10);
        }
    }

    public void CheckHealth()
    {
        if (health <= 0)
        {
            health = 0;
            ZombieHealthbarController.Instance.SetHealthBar(health, maxHealth);

            Die();
        }
        if (health >= maxHealth)
        {
            health = maxHealth;
            ZombieHealthbarController.Instance.SetHealthBar(health, maxHealth);

        }
    }

    public void SetHealth(float newHealth)
    {
        health = newHealth;
        ZombieHealthbarController.Instance.SetHealthBar(health, maxHealth);
        CheckHealth();
    }

    public void Die()
    {
        isDead = true;
        Debug.Log("Dead");
        animator.SetTrigger("ZombieDead");
        ZombieController.Instance.agent.speed = 0;
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
