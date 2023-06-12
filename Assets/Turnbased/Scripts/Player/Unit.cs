using Turnbased.Scripts.UI;
using Turnbased.Scripts.Utils;
using UnityEngine;

namespace Turnbased.Scripts.Player
{
    public class Unit : MonoBehaviour
    {
        public CharacterData charData;
        private Damagable _damagable;

        [SerializeField]private PlayerDetailsUI playerDetailsUI;
        // Start is called before the first frame update
        void Start()
        {
            _damagable = GetComponent<Damagable>();
            playerDetailsUI.SetDetails(charData.characterName);
        }

        public void TakeDamage(DamageInfo info)
        {
            _damagable.Damage(info);
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
