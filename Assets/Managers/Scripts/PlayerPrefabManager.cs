using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrefabManager : MonoBehaviour
{
    public GameObject[] playerPrefabs = new GameObject[4];

    public GameObject GetPlayer(int i)
    {
        return playerPrefabs[i];
    }
}
