using System;
using Photon.Pun;
using Turnbased.Scripts.Player;
using Unity.Mathematics;
using UnityEngine;

namespace Turnbased.Scripts.Managers
{
    public class NetworkSpawner : MonoBehaviourPunCallbacks
    {
        [SerializeField] private Transform playerSpawnPoint, opponentSpawnPoint;

        GameObject player;
        private PhotonView _photonView;
        public void Start()
        {
            _photonView = GetComponent<PhotonView>();
            SpawnPlayer();
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
                player = PhotonNetwork.Instantiate(CharacterDatabase.GetInstance().GetCharacterWithID(PlayerCharacterManager.GetInstance().GetUserCharacterID()).name, playerSpawnPoint.position, Quaternion.identity);
                Unit myUnint = player.GetComponent<Unit>();
                myUnint.turnIndex = 0;
            }
            else
            {
                // Spawn the opponent at the opponent spawn position
                player = PhotonNetwork.Instantiate(CharacterDatabase.GetInstance().GetCharacterWithID(PlayerCharacterManager.GetInstance().GetUserCharacterID()).name, opponentSpawnPoint.position, Quaternion.identity);
                Unit myUnint = player.GetComponent<Unit>();
                myUnint.turnIndex = 1;

            }
            
        }
        
        
    }
}
