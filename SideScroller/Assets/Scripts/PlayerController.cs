using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float playerSpeed = 2.5f;
    [SerializeField] private float jumpHeight = 5f;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform target;
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private LayerMask mouseAimMask;
    [SerializeField] private LayerMask groundMask;

    private Rigidbody rb;
    private Animator animator;
    private bool isGrounded;
    private float h;
    private Camera mainCamera;
    
    private int FacingSign
    {
        get
        {
            Vector3 perp = Vector3.Cross(transform.forward, Vector3.forward);
            float dir = Vector3.Dot(perp, transform.up);
            return dir > 0f ? -1 : dir < 0f ? 1 : 0;
        }
    }

    private void Start()
    {
        GetReferences();
    }

    private void Update()
    {
        h = Input.GetAxis("Horizontal");

        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit, Mathf.Infinity, mouseAimMask))
        {
            target.position = hit.point;
        }

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.velocity = new Vector3(rb.velocity.x, 0, 0);
            rb.AddForce(Vector3.up * Mathf.Sqrt(jumpHeight * -1 * Physics.gravity.y), ForceMode.VelocityChange);
        }
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector3(h * playerSpeed, rb.velocity.y, 0); //Movement
        animator.SetFloat("speed", (FacingSign * rb.velocity.x) / playerSpeed);

        rb.MoveRotation(Quaternion.Euler(new Vector3(0, 90 * Mathf.Sign(target.position.x - transform.position.x), 0))); //Face where you aim

        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundMask, QueryTriggerInteraction.Ignore);//Check if grounded
        animator.SetBool("isGrounded", isGrounded);
    }

    private void OnAnimatorIK()
    {
        animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
        animator.SetIKPosition(AvatarIKGoal.RightHand, target.position);

        animator.SetLookAtWeight(1);
        animator.SetLookAtPosition(target.position);
    }

    private void GetReferences()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        mainCamera = Camera.main;
    }
}
