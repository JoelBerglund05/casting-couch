using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Projectile : NetworkBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float time_before_destroy = 3f;


    private Rigidbody2D rb;

    private Transform target;



    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        SetDestroyTime();

        SetStraightVelocity();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void SetStraightVelocity()
    {
        rb.velocity = transform.right * speed;
    }

    void SetDestroyTime()
    {
        Destroy(gameObject, time_before_destroy);
    }
}
