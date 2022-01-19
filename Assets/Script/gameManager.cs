using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class gameManager : MonoBehaviourPunCallbacks
{
    public static gameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<gameManager>();
            }

            return instance;
        }
    }

    private static gameManager instance;

    public Transform[] spawnPositions;
    public GameObject PlayerPrefab;

    

    private void Start()
    {
        spawnPlayer();
    }

    private void spawnPlayer()
    {
        var localPlayerIndex = PhotonNetwork.LocalPlayer.ActorNumber - 1;
        var spawnPosition = spawnPositions[localPlayerIndex % spawnPositions.Length];

        PhotonNetwork.Instantiate(PlayerPrefab.name, spawnPosition.position, spawnPosition.rotation);
    }
}
