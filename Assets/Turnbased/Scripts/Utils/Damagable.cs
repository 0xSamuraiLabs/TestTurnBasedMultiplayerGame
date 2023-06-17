using System;
using NaughtyAttributes;
using UnityEngine;

namespace Turnbased.Scripts.Utils
{
    public class Damagable : MonoBehaviour
    {
        public float maxHealth;
        [SerializeField]private float currentHealth;
        public event Action OnPlayerDead;
        public event Action OnDamaged;
        public void Start()
        {
            currentHealth = maxHealth;
        }

        public float GetCurrentHealth()
        {
            return currentHealth;
        }

        [Button("Test Damage")]
        public void TestDamage()
        {
            Damage(new DamageInfo()
            {
                damageAmount = 10,
                isCritical = false
            });
        }
        
        public void Damage(DamageInfo damageInfo)
        {
            if (damageInfo != null)
            {
                if (currentHealth <= 0)
                {
                    Debug.Log("Player is dead");
                    OnPlayerDead?.Invoke();
                    return;
                }
                currentHealth -= damageInfo.damageAmount;
                OnDamaged?.Invoke();
            }
        }
        
        public void Damage(float damage)
        {
            if (currentHealth <= 0)
            {
                    Debug.Log("Player is dead");
                    OnPlayerDead?.Invoke();
                    return;
            }
            currentHealth -= damage;
            OnDamaged?.Invoke();
            if (currentHealth <= 0)
            {
                Debug.Log("Player is dead");
                OnPlayerDead?.Invoke();
            }
        }

        public void IncreaseHealth(float amt)
        {
            if (currentHealth >= maxHealth)
            {
                Debug.Log("Max health already so no use");
                return;
            }
            currentHealth += amt;
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
            OnDamaged?.Invoke();
        }
    }
}

[Serializable]
public class DamageInfo
{
    public float damageAmount;
    public bool isCritical;
}