using TMPro;
using UnityEngine;

namespace Turnbased.Scripts.UI
{
    public class PlayerDetailsUI : MonoBehaviour
    {
        [SerializeField]private TMP_Text charName;
        // Start is called before the first frame update
        public void SetDetails(string name)
        {
            charName.text = name;
        }
    }
}

