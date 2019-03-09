using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathOnExit : MonoBehaviour
{
    OnPlayerDeath deathScript;
    bool quitting;

    private void Start()
    {
        deathScript = FindObjectOfType<OnPlayerDeath>();
        Application.quitting += GameIsQuitting;
    }

    void GameIsQuitting()
    {
        quitting = true;
    }

    private void OnBecameInvisible()
    {
        if (!quitting)
            deathScript.KillPlayer(gameObject);
    }
}
