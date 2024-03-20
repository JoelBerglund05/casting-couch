using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class playerMovement : NetworkBehaviour
{
    public float moveSpeed = 5f;

    public Rigidbody2D rb;

    Vector2 movement;

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
