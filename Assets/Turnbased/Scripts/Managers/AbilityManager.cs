using System;
using DG.Tweening;
using Turnbased.Scripts.UI;
using UnityEngine;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

namespace Turnbased.Scripts.Managers
{
    public class AbilityManager : MonoBehaviour
    {
        public float abilityValue;
        [SerializeField] private Image slider;
        public float time=2f, repeat=5f;
        private void Start()
        {
            CharacterSwapUI.OnCharacterSwapped += OnCharacterSwapped;
            InvokeRepeating(nameof(DecreaseAbility), time, repeat);
        }

        private void OnCharacterSwapped(int obj)
        {
            ResetAbility();
            InvokeRepeating(nameof(DecreaseAbility), time, repeat);
        }

        public void IncreaseAbility(float ability)
        {
            if (abilityValue >= 100)
            {
                return;
            }
            abilityValue += ability;
            abilityValue = Mathf.Clamp(abilityValue, 0, 100);
            slider.DOFillAmount((abilityValue / 100),.5f);
        }
        
        
        public void DecreaseAbility()
        {
            abilityValue--;
        
            if (abilityValue <= 0)
            {
                CancelInvoke(nameof(DecreaseAbility));
                Debug.Log("Value reached or went below 0. Stopping the decrease process.");
            }
            slider.DOFillAmount((abilityValue / 100),.5f);
        }

        public void ResetAbility()
        {
            abilityValue = 100;
            slider.DOFillAmount((abilityValue / 100),.5f);
        }

        public float GetAbility()
        {
            return (abilityValue/100);
        }
    }
}
