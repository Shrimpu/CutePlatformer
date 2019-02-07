using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerPrefabManager))]
public class PlayerManager : MonoBehaviour
{
    public static int players;
    PlayerPrefabManager prefabManager;
    Vector3 spawnPoint;

    bool playerSpawned1;
    bool playerSpawned2;
    bool playerSpawned3;
    bool playerSpawned4;
    bool playerSpawned5;
    bool playerSpawned6;

    private void Start()
    {
        prefabManager = GetComponent<PlayerPrefabManager>();
        spawnPoint = GameObject.FindGameObjectWithTag("Respawn").transform.position;
    }

    void Update()
    {
        if (Input.GetButtonDown(InputManager.CustomInputs.P1Keyboard.jump) && players < 4 && !playerSpawned1)
        {
            SpawnPlayer(1);
        }
        if (Input.GetButtonDown(InputManager.CustomInputs.P2Keyboard.jump) && players < 4 && !playerSpawned2)
        {
            SpawnPlayer(2);
        }
        if (Input.GetButtonDown(InputManager.CustomInputs.P1.jump) && players < 4 && !playerSpawned3)
        {
            SpawnPlayer(3);
        }
        if (Input.GetButtonDown(InputManager.CustomInputs.P2.jump) && players < 4 && !playerSpawned4)
        {
            SpawnPlayer(4);
        }
        if (Input.GetButtonDown(InputManager.CustomInputs.P3.jump) && players < 4 && !playerSpawned5)
        {
            SpawnPlayer(5);
        }
        if (Input.GetButtonDown(InputManager.CustomInputs.P4.jump) && players < 4 && !playerSpawned6)
        {
            SpawnPlayer(6);
        }
    }

    public void SpawnPlayer(int i)
    {
        if (spawnPoint != null)
        {
            GameObject spawnedPlayer = Instantiate(prefabManager.GetPlayer(players), spawnPoint, Quaternion.identity);
            Movement m = spawnedPlayer.GetComponent<Movement>();
            m.playerInputID = i;
            m.playerID = players;

            players++;

            if (i == 1)
                playerSpawned1 = true;
            if (i == 2)
                playerSpawned2 = true;
            if (i == 3)
                playerSpawned3 = true;
            if (i == 4)
                playerSpawned4 = true;
            if (i == 5)
                playerSpawned5 = true;
            if (i == 6)
                playerSpawned6 = true;
        }
        else
            Debug.LogWarning("No Spawnpoint in scene");
    }

    #region Remove Player Buttons

    public void RemovePlayer1()
    {
        RemovePlayer(1);
    }
    public void RemovePlayer2()
    {
        RemovePlayer(2);
    }
    public void RemovePlayer3()
    {
        RemovePlayer(3);
    }
    public void RemovePlayer4()
    {
        RemovePlayer(4);
    }

    void RemovePlayer(int i)
    {
        Movement[] player = FindObjectsOfType<Movement>();

        if (player[i] != null)
        {
            if (player[i].playerInputID == 1)
                playerSpawned1 = false;
            if (player[i].playerInputID == 2)
                playerSpawned2 = false;
            if (player[i].playerInputID == 3)
                playerSpawned3 = false;
            if (player[i].playerInputID == 4)
                playerSpawned4 = false;
            if (player[i].playerInputID == 5)
                playerSpawned5 = false;
            if (player[i].playerInputID == 6)
                playerSpawned6 = false;

            players--;

            Destroy(player[i].gameObject);
        }
    }

    #endregion
}
