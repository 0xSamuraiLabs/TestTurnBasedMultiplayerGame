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
        // Start is called before the first frame update
        void Start()
        {
            _damagable = GetComponent<Damagable>();
            playerDetailsUI.SetDetails(charData.characterName);
            pv = GetComponent<PhotonView>();
        }


        public void Attack()
        {
            TakeDamage(charData.DamageInfo);
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

        public bool IsMyTurn(Unit unit)
        {
            return unit == this; 
        }

        public DamageInfo GetDamage()
        {
            return charData.DamageInfo;
        }
    
    }
}
