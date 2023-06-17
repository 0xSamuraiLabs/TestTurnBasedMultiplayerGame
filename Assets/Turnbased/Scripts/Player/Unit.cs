using System;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using Turnbased.Scripts.Managers;
using Turnbased.Scripts.UI;
using Turnbased.Scripts.Utils;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Turnbased.Scripts.Player
{
    public class Unit : MonoBehaviourPunCallbacks
    {
        public CharacterData charData;
        public Damagable _damagable;
        private PhotonView pv;
        private PlayerMessage _playerMessage;
        [SerializeField]private PlayerDetailsUI playerDetailsUI;
        [SerializeField] private Transform characterModelSpawnPoint;
        public bool isDefending;
        public int currentCharacter;
        public Animator _animator;

        private GameObject charModel;

        private AbilityManager abilityManager;

        public event Action OnPlayerLeft;  
        // Start is called before the first frame update
        void Start()
        {
            _damagable = GetComponent<Damagable>();
            _playerMessage = GetComponent<PlayerMessage>();
            _damagable.OnPlayerDead += OnPlayerDead;
            abilityManager = FindObjectOfType<AbilityManager>();
            pv = GetComponent<PhotonView>();
            
            if (pv.IsMine)
            {
                SpawnCharacter(PlayerCharacterManager.GetInstance().GetUserCharacterID()[0]); 
            }
            
            FlipCharacter();

        }
        

        Unit GetOpponentPlayer()
        {
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            Unit unit=null;
            foreach (var pla in players)
            {
                if (!pla.GetComponent<PhotonView>().IsMine)
                {
                    return pla.GetComponent<Unit>();
                }
            }

            return null;
        }

        void FlipCharacter()
        {
            if (pv.IsMine && PhotonNetwork.IsMasterClient)
            {
                transform.Find("CharacterSpawnPosition").Rotate(Vector3.up, 180f); // Rotate the character 180 degrees around the Y-axis
            }
            else
            {
                transform.Find("CharacterSpawnPosition").Rotate(Vector3.up, 0f); // Rotate the character 180 degrees around the Y-axis
            }
        }
        

        private void OnPlayerDead()
        {
            _animator.SetInteger("State",9);
        }

        public float GetMyAbility()
        {
            return abilityManager.GetAbility();
        }

        public void SpawnCharacter(int id)
        {
            currentCharacter = id;
            CharacterHealthData characterHealthData =
                PlayerCharacterManager.GetInstance().GetCharacterHealthData(currentCharacter);

            pv.RPC(nameof(InitCharacter),RpcTarget.All,id);
            pv.RPC(nameof(SetHealth),RpcTarget.All,characterHealthData.health);


        }

        public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
        {
            base.OnPlayerLeftRoom(otherPlayer);
            if (PhotonNetwork.CurrentRoom.PlayerCount < 2)
            {
                OnPlayerLeft?.Invoke();
            }
        }

        [PunRPC]
        void SetHealth(float health)
        {
            if (_damagable)
            {
                _damagable.SetHealth(health);
            }
        }

        [PunRPC]
        void InitCharacter(int index)
        {
            if (charModel != null)
            {
                Destroy(charModel);
            }
            GameObject characterModel = Instantiate(CharacterDatabase.GetInstance()
                .GetCharacterWithID(index).characterPrefab,characterModelSpawnPoint.transform);
            characterModel.transform.localScale = Vector3.one;
            characterModel.transform.localPosition = Vector3.one;
            charData = CharacterDatabase.GetInstance()
                .GetCharacterWithID(index).CharacterData;
            playerDetailsUI.SetDetails(charData.characterName);
            charModel = characterModel;
            
        }
        
        public void PlayAttackAnimation()
        {
            pv.RPC(nameof(AttackAnimationRPC),RpcTarget.All);
        }

        [PunRPC]
        void AttackAnimationRPC()
        {
            if (_animator == null)
            {
                _animator = this.transform.Find("CharacterSpawnPosition").GetComponentInChildren<Animator>();
            }
            
            _animator.SetTrigger("Attack");
        }
        
        
       
        
        public void Attack(DamageInfo damageInfo , bool isCritical=false)
        {
            damageInfo.isCritical = isCritical;
            TakeDamage(damageInfo);
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
            float potionFailChance = 0.001f * (1 + GetMyAbility());
            float potionSuperEffectiveChance = 0.0001f * (1 - GetMyAbility());

            float random = Random.Range(0f, 1f);

            if (random <= potionFailChance)
            {
                abilityManager.IncreaseAbility(30);
                Debug.Log("Potion failed! It didn't work at all.");
            }
            else if (random <= potionFailChance + potionSuperEffectiveChance)
            {
                Debug.Log("Potion was super effective! Completely healed!");
                pv.RPC(nameof(HealRPC),RpcTarget.AllBuffered,_damagable.maxHealth);
                PlayerCharacterManager.GetInstance().SaveCharacterHealth(currentCharacter,_damagable.GetCurrentHealth());
            }
            else
            {
                Debug.Log("Potion used successfully!");
                abilityManager.IncreaseAbility(30);
                pv.RPC(nameof(HealRPC),RpcTarget.AllBuffered,amt);
                PlayerCharacterManager.GetInstance().SaveCharacterHealth(currentCharacter,_damagable.GetCurrentHealth());
            }
            
            Defend(false);
        }

        [PunRPC]
        void HealRPC(float amt)
        {
            if (amt >= _damagable.maxHealth)
            {
                _playerMessage.ShowMessage("Super Heal");
            }
            else
            {
                _playerMessage.ShowMessage("Health++");
            }
            _damagable.IncreaseHealth(amt);
        }
        
        private void TakeDamage(DamageInfo info)
        {
            if (info.isCritical)
            {
                info.damageAmount *= 3;
                _playerMessage.ShowMessage("Critical");
            }
            
            pv.RPC(nameof(TakeDamageRPC),RpcTarget.AllBuffered,info.damageAmount);
            if (!isDefending)
            {
                pv.RPC(nameof(SendDamage),RpcTarget.Others);
            }
        }

        [PunRPC]
        void SendDamage()
        {
            PlayerCharacterManager.GetInstance().SaveCharacterHealth(currentCharacter,_damagable.GetCurrentHealth());
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
