using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieController : MonoBehaviour
{
    public static ZombieController Instance { get; set; }
    public int zombieDamage;

    [HideInInspector] public NavMeshAgent agent = null;
    private Animator animator;
    [SerializeField] private Transform target;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        GetReferences();
    }

    private void Update()
    {
        MoveToTarget();
    }

    private void MoveToTarget()
    {
        agent.SetDestination(target.position);
        animator.SetFloat("MoveSpeed", 1f, 0.3f, Time.deltaTime);

        var distance = Vector3.Distance(target.position, transform.position);
        if (distance <= agent.stoppingDistance)
        {
            animator.SetFloat("MoveSpeed", 0f);
        }
    }

    private void GetReferences()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }
}
