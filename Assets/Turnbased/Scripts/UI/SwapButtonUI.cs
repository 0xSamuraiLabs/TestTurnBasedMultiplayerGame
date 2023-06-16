using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SwapButtonUI : MonoBehaviour
{
   [SerializeField] private TMP_Text text;
   public Button btn;

   public void SetCharacterName(string name)
   {
      text.text = name;
   }

   private void OnDestroy()
   {
      btn.onClick.RemoveAllListeners();
   }
}
