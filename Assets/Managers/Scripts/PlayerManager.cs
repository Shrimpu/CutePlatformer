using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static event System.Action PlayersInSceneChanged;

    public PlayerPrefabs playerPrefabs;

    public GameObject spawnEffect;
    public bool canSpawnPlayer = true;

    public static int players;
    Vector3 spawnPoint;

    bool playerSpawned1;
    bool playerSpawned2;
    bool playerSpawned3;
    bool playerSpawned4;
    bool playerSpawned5;
    bool playerSpawned6;

    private void Start()
    {
        spawnPoint = GameObject.FindGameObjectWithTag("Respawn").transform.position;
        players = 0;
    }

    void Update()
    {
        if (canSpawnPlayer)
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
    }

    public void SpawnPlayer(int i)
    {
        if (spawnPoint == Vector3.zero)
        {
            Debug.LogWarning("No Spawnpoint in scene");
            spawnPoint = GameObject.FindGameObjectWithTag("Respawn").transform.position;
        }

        if (spawnPoint != Vector3.zero)
        {
            GameObject spawnedPlayer = Instantiate(playerPrefabs.GetPlayer(players), spawnPoint, Quaternion.identity);
            if (spawnEffect != null)
                Instantiate(spawnEffect, spawnedPlayer.transform.position, Quaternion.identity);
            Movement m = spawnedPlayer.GetComponent<Movement>();
            m.playerInputID = i;
            //m.playerID = players;

            players++;

            switch (i)
            {
                case 1:
                    playerSpawned1 = true;
                    m.horizontal = InputManager.CustomInputs.P1Keyboard.horizontal;
                    m.jump = InputManager.CustomInputs.P1Keyboard.jump;
                    m.crouch = InputManager.CustomInputs.P1Keyboard.crouch;
                    break;
                case 2:
                    playerSpawned2 = true;
                    m.horizontal = InputManager.CustomInputs.P2Keyboard.horizontal;
                    m.jump = InputManager.CustomInputs.P2Keyboard.jump;
                    m.crouch = InputManager.CustomInputs.P2Keyboard.crouch;
                    break;
                case 3:
                    playerSpawned3 = true;
                    m.horizontal = InputManager.CustomInputs.P1.horizontal;
                    m.jump = InputManager.CustomInputs.P1.jump;
                    m.crouch = InputManager.CustomInputs.P1.crouch;
                    break;
                case 4:
                    playerSpawned4 = true;
                    m.horizontal = InputManager.CustomInputs.P2.horizontal;
                    m.jump = InputManager.CustomInputs.P2.jump;
                    m.crouch = InputManager.CustomInputs.P2.crouch;
                    break;
                case 5:
                    playerSpawned5 = true;
                    m.horizontal = InputManager.CustomInputs.P3.horizontal;
                    m.jump = InputManager.CustomInputs.P3.jump;
                    m.crouch = InputManager.CustomInputs.P3.crouch;
                    break;
                case 6:
                    playerSpawned6 = true;
                    m.horizontal = InputManager.CustomInputs.P4.horizontal;
                    m.jump = InputManager.CustomInputs.P4.jump;
                    m.crouch = InputManager.CustomInputs.P4.crouch;
                    break;
                default:
                    Debug.LogError("Inputs not assigned!");
                    break;
            }

            PlayersInSceneChanged?.Invoke();
        }
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

            PlayersInSceneChanged?.Invoke();
        }
    }

    #endregion

    [System.Serializable]
    public class PlayerPrefabs
    {
        public GameObject[] playerPrefabs = new GameObject[4];

        public GameObject GetPlayer(int i)
        {
            return playerPrefabs[i];
        }
    }
}
