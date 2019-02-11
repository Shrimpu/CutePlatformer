using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class OnStomped : MonoBehaviour
{
    public float launchForce = 5f;
    public GameObject collisionEffect;

    Movement playerMovement;
    OnPlayerDeath playerdeath;
    GameObject player;

    private void Start()
    {
        playerdeath = FindObjectOfType<OnPlayerDeath>();
        player = transform.parent.transform.parent.gameObject; // gets the correct gameObject
        playerMovement = player.GetComponent<Movement>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Feet"))
        {
            if (collisionEffect != null)
                Instantiate(collisionEffect, collision.transform.position, Quaternion.identity);

            Rigidbody2D colrb = collision.transform.parent.GetComponent<Rigidbody2D>();
            colrb.velocity = new Vector2(colrb.velocity.x, launchForce);

            if (!playerMovement.isCrouching)
                playerdeath.KillPlayer(player);
        }
    }
}
