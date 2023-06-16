using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

namespace Turnbased.Scripts.UI
{
    public class CharacterSwapUI : MonoBehaviour
    {
        [SerializeField] private Transform buttonSpawnLocation;
        [SerializeField] private SwapButtonUI buttonPrefab;
        public static event Action<int> OnCharacterSwapped;
        private List<GameObject> generatedUI = new List<GameObject>();
        public void FeedUI(List<int> characters)
        {
            if (generatedUI != null)
            {
                foreach (var gI in generatedUI)
                {
                    Destroy(gI);
                }
            }
            for (int i = 0; i < characters.Count; i++)
            {
                SwapButtonUI swapBtn = Instantiate(buttonPrefab, buttonSpawnLocation);
                swapBtn.SetCharacterName(characters[i].ToString());
                var x = i;
                swapBtn.btn.onClick.AddListener(delegate { OnClick(x); });
                generatedUI.Add(swapBtn.gameObject);
            }
        }

        void OnClick(int i)
        {
            OnCharacterSwapped?.Invoke(i);
            gameObject.SetActive(false);
        }

   
    }
}
