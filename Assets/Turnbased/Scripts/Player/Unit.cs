using Photon.Pun;
using Turnbased.Scripts.UI;
using Turnbased.Scripts.Utils;
using UnityEngine;

namespace Turnbased.Scripts.Player
{
    public class Unit : MonoBehaviour
    {
        public CharacterData charData;
        private Damagable _damagable;
        private PhotonView pv;
        [SerializeField]private PlayerDetailsUI playerDetailsUI;
        [SerializeField] private MoveData _moveData;

        public bool isDefending;
        // Start is called before the first frame update
        void Start()
        {
            _damagable = GetComponent<Damagable>();
            playerDetailsUI.SetDetails(charData.characterName);
            pv = GetComponent<PhotonView>();
        }
        
        
        public void Attack()
        {
            if (isDefending)
            {
                Debug.Log("Deflected");
                Defend(false);
                return;
            }
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
            _damagable.IncreaseHealth(amt);
        }
        
        private void TakeDamage(DamageInfo info)
        {
            pv.RPC(nameof(TakeDamageRPC),RpcTarget.AllBuffered,info.damageAmount);
        }

        [PunRPC]
        void TakeDamageRPC(float val)
        {
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
