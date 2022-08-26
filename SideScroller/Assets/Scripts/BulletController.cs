using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    #region VARIABLES

    public static BulletController Instance { get; set; }

    [SerializeField] private float bulletVelocity = 20f;
    [SerializeField] private float bulletLifeTime = 1f;

    private int fireLayer;
    private float lifeTimer;

    #endregion VARIABLES

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        RaycastHit hit;
        if(Physics.Raycast(transform.position, transform.forward, out hit, bulletVelocity * Time.deltaTime, ~(1 << fireLayer)))
        {
            transform.position = hit.point;
            Vector3 reflected = Vector3.Reflect(transform.forward, hit.normal); //for impact effects
            Vector3 direction = transform.forward;
            Vector3 vop = Vector3.ProjectOnPlane(reflected, Vector3.forward);
            transform.forward = vop;
            transform.rotation = Quaternion.LookRotation(vop, Vector3.forward);
            Hit(transform.position, direction, reflected, hit.collider);
        }
        else
        {
            transform.Translate(Vector3.forward * bulletVelocity * Time.deltaTime);
        }

        if(Time.time > lifeTimer + bulletLifeTime)
        {
            Destroy(gameObject);
        }
    }

    private void Hit(Vector3 position, Vector3 direction, Vector3 reflected, Collider collider)
    {
        //When object hits a collider

        Destroy(gameObject);
    }

    public  void Fire(Vector3 position, Vector3 rotation, int layer)
    {
        lifeTimer = Time.time;
        transform.position = position;
        transform.eulerAngles = rotation;
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);

        Vector3 vop = Vector3.ProjectOnPlane(transform.forward, Vector3.forward);
        transform.forward = vop;
        transform.rotation = Quaternion.LookRotation(vop, Vector3.forward);
    }
}
