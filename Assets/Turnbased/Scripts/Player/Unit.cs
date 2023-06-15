using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using Turnbased.Scripts.Managers;
using Turnbased.Scripts.UI;
using Turnbased.Scripts.Utils;
using UnityEngine;

namespace Turnbased.Scripts.Player
{
    public class Unit : MonoBehaviourPunCallbacks
    {
        public CharacterData charData;
        private Damagable _damagable;
        private PhotonView pv;
        private PlayerMessage _playerMessage;
        [SerializeField]private PlayerDetailsUI playerDetailsUI;
        [SerializeField] private Transform characterModelSpawnPoint;
        public bool isDefending;
        // Start is called before the first frame update
        void Start()
        {
            _damagable = GetComponent<Damagable>();
            _playerMessage = GetComponent<PlayerMessage>();
            pv = GetComponent<PhotonView>();
            
            if (pv.IsMine)
            {
                SpawnCharacter(PlayerCharacterManager.GetInstance().GetUserCharacterID()[2]); 
            }
        }

        public void SpawnCharacter(int id)
        {
            pv.RPC(nameof(InitCharacter),RpcTarget.All,id);
        }
        
        [PunRPC]
        void InitCharacter(int index)
        {
            GameObject characterModel = Instantiate(CharacterDatabase.GetInstance()
                .GetCharacterWithID(index).characterPrefab,characterModelSpawnPoint.transform);
            characterModel.transform.localScale = Vector3.one;
            characterModel.transform.localPosition = Vector3.one;
            charData = CharacterDatabase.GetInstance()
                .GetCharacterWithID(index).CharacterData;
            playerDetailsUI.SetDetails(charData.characterName);
        }

       
        
        public void Attack()
        {
            TakeDamage(charData.DamageInfo);
            Defend(false);
        }

        public void Defend(bool state)
        {
            pv.RPC(nameof(DefendRPC),RpcTarget.All,state);
        }

        [PunRPC]
        void DefendRPC(bool state)
        {
            isDefending = state;
        }

        public void Heal(float amt)
        {
            pv.RPC(nameof(HealRPC),RpcTarget.AllBuffered,amt);
            Defend(false);
        }

        [PunRPC]
        void HealRPC(float amt)
        {
            _playerMessage.ShowMessage("Health++");
            _damagable.IncreaseHealth(amt);
        }
        
        private void TakeDamage(DamageInfo info)
        {
            pv.RPC(nameof(TakeDamageRPC),RpcTarget.AllBuffered,info.damageAmount);
        }

        [PunRPC]
        void TakeDamageRPC(float val)
        {
            if (isDefending)
            {
                _playerMessage.ShowMessage("Deflected");
                Defend(false);
                return;
            }
            _damagable.Damage(val);
        }
        
        public bool IsFainted()
        {
            return _damagable.GetCurrentHealth() <= 0;
        }
        
        public DamageInfo GetDamage()
        {
            return charData.DamageInfo;
        }
        
    }
}
