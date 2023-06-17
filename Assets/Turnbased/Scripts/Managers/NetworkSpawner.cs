using System;
using System.Collections.Generic;
using Photon.Pun;
using Turnbased.Scripts.Player;
using Unity.Mathematics;
using UnityEngine;

namespace Turnbased.Scripts.Managers
{
    public class NetworkSpawner : MonoBehaviourPunCallbacks 
    {
        [SerializeField] private Transform playerSpawnPoint, opponentSpawnPoint;
        private Camera _camera;
        GameObject player;
        private PhotonView _photonView;
        
        public void Start()
        {
            _camera = Camera.main;
            _photonView = GetComponent<PhotonView>();
            SpawnPlayer();
        }

        public Transform GetSpawnPlayerPosition()
        {
            return playerSpawnPoint;
        }
        
        public Transform GetSpawnOpponentPosition()
        {
            return opponentSpawnPoint;
        }

        
        void SpawnPlayer()
        {
            if (player != null)
            {
                return;
            }

            // Check if the local player is the master client
            if (PhotonNetwork.IsMasterClient)
            {
                // Spawn the local player at the player spawn position
                player = PhotonNetwork.Instantiate(CharacterDatabase.GetInstance().CharacterPrefab.name, playerSpawnPoint.position, Quaternion.identity);
            }
            else
            {
                // Spawn the opponent at the opponent spawn position
                player = PhotonNetwork.Instantiate(CharacterDatabase.GetInstance().CharacterPrefab.name, opponentSpawnPoint.position, Quaternion.identity);
                // _camera.orthographicSize = -_camera.orthographicSize;
                // player.transform.localScale = -(player.transform.localScale);
            }
        }
        

        

        
    }
}
