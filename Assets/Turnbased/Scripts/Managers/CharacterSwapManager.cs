using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Turnbased.Scripts.Managers;
using Turnbased.Scripts.UI;
using UnityEngine;

public class CharacterSwapManager : MonoBehaviour
{
    [SerializeField] private CharacterSwapUI _characterSwapUI;
    public List<int> charList;
    
    public void StartSwapProcess(int activeCharacter)
    {
        List<int> tempArray = PlayerCharacterManager.GetInstance().GetUserCharacterID().ToList();
        for (int i = 0; i < tempArray.Count; i++)
        {
            if (tempArray[i] == activeCharacter)
            {
                tempArray.Remove(activeCharacter);
            }
        }

        charList = tempArray;
        _characterSwapUI.gameObject.SetActive(true);
        _characterSwapUI.FeedUI(charList);
    }
}
