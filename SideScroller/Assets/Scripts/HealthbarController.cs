using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthbarController : MonoBehaviour
{
    public static HealthbarController Instance { get; set;}
    public GameObject following;
    [Range(0.0f, 1.0f)]
    public float interested;
    public Vector3 offset;
    public PlayerStats stats;
    public Slider slider;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        SetHealth();
    }

    void LateUpdate()
    {
        transform.position = Vector3.MoveTowards(transform.position, following.transform.position + offset, interested);
        slider.value = stats.health / 100;
    }

    private void SetHealth()
    {
        slider.maxValue = stats.maxHealth;
        slider.value = stats.maxHealth;
    }
}
