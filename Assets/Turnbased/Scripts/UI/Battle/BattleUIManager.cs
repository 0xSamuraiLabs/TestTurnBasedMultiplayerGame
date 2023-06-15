using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BattleUIManager : MonoBehaviour
{
    [SerializeField] private TMP_Text battleMessageText;
    [SerializeField] private CanvasGroup battleActions;

    public void SetBattleActions(bool state)
    {
        battleActions.interactable = state;
    }

    public void ShowIncomingBattleText(string msg)
    {
        battleMessageText.text =
            msg;
    }
}
