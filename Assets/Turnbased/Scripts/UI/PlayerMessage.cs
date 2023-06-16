using System.Collections;
using TMPro;
using UnityEngine;

namespace Turnbased.Scripts.UI
{
    public class PlayerMessage : MonoBehaviour
    {
        [SerializeField] private TMP_Text messageText;

        public void ShowMessage(string msg)
        {
            messageText.gameObject.SetActive(true);
            messageText.text = msg;
            StartCoroutine(DisableAfterTime());
        }

        IEnumerator DisableAfterTime()
        {
            yield return new WaitForSeconds(0.9f);
            messageText.gameObject.SetActive(false);
        }
    
    }
}
