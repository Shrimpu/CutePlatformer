using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Sawblade : MonoBehaviour
{
    OnPlayerDeath playerDeath;

    private void Start()
    {
        playerDeath = FindObjectOfType<OnPlayerDeath>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerDeath.KillPlayer(collision.gameObject);
        }
    }
}
