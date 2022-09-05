using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance { get; set; }
    public int weaponDamage = 10;

    [SerializeField] private float playerSpeed = 2.5f;
    [SerializeField] private float jumpHeight = 5f;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform target;
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private LayerMask mouseAimMask;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private GameObject bulletPrefab, bulletSpawnPosition;

    [SerializeField] private AnimationCurve recoilCurve;
    [SerializeField] private float recoilDuration = 0.25f;
    [SerializeField] private float recoilMaxRotation = 45f;
    [SerializeField] private Transform rightForeArm, rightHand;

    
    private Rigidbody rb;
    private Animator animator;
    private bool isGrounded;
    private float h;
    private float recoilTimer;
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
        if (PlayerStats.Instance.isDead == false)
        {
            h = Input.GetAxis("Horizontal");

            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, mouseAimMask))
            {
                target.position = hit.point;
            }

            if (Input.GetButtonDown("Jump") && isGrounded)
            {
                rb.velocity = new Vector3(rb.velocity.x, 0, 0);
                rb.AddForce(Vector3.up * Mathf.Sqrt(jumpHeight * -1 * Physics.gravity.y), ForceMode.VelocityChange);
                animator.SetBool("isJumping", true);
            }

            if (Input.GetButtonDown("Fire1"))
            {
                Fire();
            }
        }
    }

    private void LateUpdate()
    {
        if (PlayerStats.Instance.isDead == false)
        {
            if (recoilTimer < 0)
            {
                return;
            }

            float curveTime = (Time.time - recoilTimer) / recoilDuration;
            if (curveTime > 1f)
            {
                recoilTimer = -1;
            }
            else
            {
                rightForeArm.Rotate(Vector3.forward, recoilCurve.Evaluate(curveTime) * recoilMaxRotation, Space.Self);
            }
        }
    }

    private void FixedUpdate()
    {
        if(PlayerStats.Instance.isDead == false)
        {
            rb.velocity = new Vector3(h * playerSpeed, rb.velocity.y, 0); //Movement
            animator.SetFloat("speed", (FacingSign * rb.velocity.x) / playerSpeed);

            if (isGrounded && ((FacingSign * rb.velocity.x) / playerSpeed) != 0)
            {
                animator.SetBool("isMoving", true);
            }
            else
            {
                animator.SetBool("isMoving", false);
            }

            rb.MoveRotation(Quaternion.Euler(new Vector3(0, 90 * Mathf.Sign(target.position.x - transform.position.x), 0))); //Face where you aim

            isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundMask, QueryTriggerInteraction.Ignore);//Check if grounded
            animator.SetBool("isGrounded", isGrounded);
            if (isGrounded)
            {
                animator.SetBool("isJumping", false);
            }
            if (rb.velocity.y < -3f)
            {
                animator.SetBool("isFalling", true);
            }
            if (rb.velocity.y > 0)
            {
                animator.SetBool("isJumping", true);
            }
            if (rb.velocity.y == 0)
            {
                animator.SetBool("isFalling", false);
            }
        }
    }

    private void OnAnimatorIK()
    {
        if (PlayerStats.Instance.isDead == false)
        {
            animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
            animator.SetIKPosition(AvatarIKGoal.RightHand, target.position);

            animator.SetLookAtWeight(1);
            animator.SetLookAtPosition(target.position);
        }
        else
        {
            animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 0);
            animator.SetLookAtWeight(0);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ZombieHand"))
        {
            animator.SetTrigger("PlayerHit");
            PlayerStats.Instance.TakeDamage(ZombieController.Instance.zombieDamage);
        }
    }
    private void Fire()
    {
        recoilTimer = Time.time;

        var go = Instantiate(bulletPrefab);
        go.transform.position = bulletSpawnPosition.transform.position;
        var bullet = go.GetComponent<BulletController>();
        BulletController.Instance.Fire(go.transform.position, bulletSpawnPosition.transform.eulerAngles, gameObject.layer);
    }
    private void GetReferences()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        mainCamera = Camera.main;

        Cursor.visible = false;
    }
}
