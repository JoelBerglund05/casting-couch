using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class playerMovement : NetworkBehaviour
{
    private float moveSpeed = 5f;

    private Rigidbody2D rb;

    private Vector2 movement;

    void Start(){
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
       movement.x = Input.GetAxisRaw("Horizontal");
       movement.y = Input.GetAxisRaw("Vertical");

      
    }

    void FixedUpdate()
    {
        if (!IsOwner) return;
        rb.MovePosition(rb.position + movement.normalized * moveSpeed * Time.deltaTime);
    }
}
