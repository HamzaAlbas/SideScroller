using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ZombieHealthbarController : MonoBehaviour
{
    public static ZombieHealthbarController Instance { get; set; }
    public GameObject following;
    [Range(0.0f, 1.0f)]
    public float interested;
    public Vector3 offset;
    public ZombieStats stats;
    public Slider slider;

    private void Awake()
    {
        Instance = this;
    }

    void LateUpdate()
    {
        transform.position = Vector3.MoveTowards(transform.position, following.transform.position + offset, interested);
    }

    public void SetHealthBar(float health, float maxHealth)
    {
        slider.maxValue = maxHealth;
        slider.value = health;
    }
}
