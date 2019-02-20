using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnPlayerDeath : MonoBehaviour
{
    public delegate void KilledPlayerInfo(GameObject player);
    public static KilledPlayerInfo PlayerKilled;

    public GameObject bloodEffect;
    public GameObject spawnEffect;
    public float respawnTime = 0.6f;
    [Space]
    public float camShakeIntensity = 0.05f;
    public int camShakes = 1;

    CameraEffects cameraEffects;
    Transform respawnPoint;

    void Start()
    {
        respawnPoint = GameObject.FindGameObjectWithTag("Respawn").transform;
        if (respawnPoint.position == Vector3.zero)
            Debug.LogError("No Respawn In Scene");
        cameraEffects = FindObjectOfType<CameraEffects>();
    }

    public void KillPlayer(GameObject player)
    {
        if (cameraEffects != null)
            cameraEffects.Shake(camShakeIntensity, camShakes);

        Movement playerMovement = player.GetComponent<Movement>();
        playerMovement.CanJump = false; // now the player doesn't jump on respawn

        StartCoroutine(PlayerKillEffect(player));
    }

    IEnumerator PlayerKillEffect(GameObject player)
    {
        if (bloodEffect != null)
            Instantiate(bloodEffect, player.transform.position, Quaternion.identity);
        player.SetActive(false);
        PlayerKilled?.Invoke(player);

        yield return new WaitForSeconds(respawnTime);

        player.transform.position = respawnPoint.position;
        player.SetActive(true);
        if (spawnEffect != null)
            Instantiate(spawnEffect, player.transform.position, Quaternion.identity);
    }
}
