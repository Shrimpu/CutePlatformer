using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathOnExit : MonoBehaviour
{
    OnPlayerDeath deathScript;

    private void Start()
    {
        deathScript = FindObjectOfType<OnPlayerDeath>();
    }

    private void OnBecameInvisible()
    {
        deathScript.KillPlayer(gameObject);
    }
}
