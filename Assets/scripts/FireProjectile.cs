using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;


public class FireProjectile : NetworkBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Transform projectile_spawn_point;
    [SerializeField] private GameObject gun;
    [SerializeField] private GameObject projectile;
    private GameObject projectile_inst;

    private Rigidbody2D rb;

    Vector2 world_position;
    Vector2 direction;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        //projectile = projectile_prefab.GetComponent<Projectile>();
    }
    void Update()
    {
        HandleRotation();
        HandleGunShootingServerRpc();
    }

    void HandleRotation()
    {
        world_position = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        direction = (world_position - (Vector2)gun.transform.position).normalized;
        gun.transform.right = direction;
    }
    [ServerRpc(RequireOwnership = false)]
    void HandleGunShootingServerRpc()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame && IsOwner)
        {
            GameObject spawn_bullet = Instantiate(projectile, projectile_spawn_point.position, gun.transform.rotation);
            spawn_bullet.GetComponent<NetworkObject>().Spawn();
        }
    }


}
