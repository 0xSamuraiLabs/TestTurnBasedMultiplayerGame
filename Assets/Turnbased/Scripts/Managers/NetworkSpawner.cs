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

        private Unit player, opponent;
        private PhotonView _photonView;
        public void Start()
        {
            _photonView = GetComponent<PhotonView>();
            SpawnPlayer();
        }
        
        void SpawnPlayer()
        {
            player = Instantiate(CharacterDatabase.GetInstance().GetCharacterWithID(PlayerCharacterManager.GetInstance().GetUserCharacterID()));
            if (PhotonNetwork.LocalPlayer.IsLocal)
            {
                player.transform.position = playerSpawnPoint.position;
            }
            photonView.RPC(nameof(SpawnOpponent),RpcTarget.All,PlayerCharacterManager.GetInstance().GetUserCharacterID());
        }
        
        [PunRPC]
        void SpawnOpponent(int characterId)
        {
            opponent = Instantiate(CharacterDatabase.GetInstance().GetCharacterWithID(characterId));
            opponent.transform.position = opponentSpawnPoint.position;
        }
    }
}
