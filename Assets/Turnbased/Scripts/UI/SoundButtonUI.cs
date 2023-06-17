using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundButtonUI : MonoBehaviour
{
    private Button btn;
    [SerializeField] private AudioClip Clip;
    private void Start()
    {
        btn = GetComponent<Button>();
        btn.onClick.AddListener(OnButtonClicked);
    }

    private void OnButtonClicked()
    {
        AudioManager.GetInstance().PlaySfx(Clip);
    }

    private void OnDestroy()
    {
        if (btn)
        {
            btn.onClick.RemoveListener(OnButtonClicked);
        }
    }
}
