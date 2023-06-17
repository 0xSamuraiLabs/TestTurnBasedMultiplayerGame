using System;
using System.Collections.Generic;
using Photon.Pun;
using Turnbased.Scripts.Player;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Turnbased.Scripts.Managers
{
    public class PlayerCharacterManager : Singleton<PlayerCharacterManager>
    {
        [SerializeField] private int[] playerId;
        [SerializeField] private List<CharacterHealthData> characterData;

        
        private void Awake()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            if (arg0.buildIndex == 1)
            {
                SetCharacterData();
            }
        }

        void SetCharacterData()
        {
            characterData.Clear();
            for (int i = 0; i < playerId.Length; i++)
            {
                CharacterHealthData characterHealthData = new CharacterHealthData();
                characterHealthData.characterIndex = playerId[i];
                characterHealthData.health = 100f;
                characterData.Add(characterHealthData);
            }
        }

        public void SaveCharacterHealth(int characterIndex,float amt)
        {
            foreach (var charda in characterData)
            {
                if (charda.characterIndex == characterIndex)
                {
                    charda.health = amt;
                }
            }
        }

        public CharacterHealthData GetCharacterHealthData(int characterIndex)
        {
            for (int i = 0; i < characterData.Count; i++)
            {
                if (characterData[i].characterIndex == characterIndex)
                {
                    return characterData[i];
                }
            }

            return null;
        }
        
        public int[] GetUserCharacterID()
        {
            return playerId;
        }
        
        
        
    }
}

[Serializable]
public class CharacterHealthData
{
    public int characterIndex;
    public float health;
}
