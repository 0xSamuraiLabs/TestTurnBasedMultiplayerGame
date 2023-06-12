using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BattleUIManager : MonoBehaviour
{
    [SerializeField] private TMP_Text battleMessageText;

    public void ShowIncomingBattleText(CharacterData characterData)
    {
        battleMessageText.text =
            characterData.characterName + " did attack of " + characterData.DamageInfo.damageAmount;
    }
}
