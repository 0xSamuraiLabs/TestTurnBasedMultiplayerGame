using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Turnbased.Scripts.Utils;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    [SerializeField] private Image fillImage;
    private Damagable _damagable;

    private void Start()
    {
        _damagable = GetComponent<Damagable>();
        _damagable.OnDamaged += OnDamaged;
    }
    private void OnDamaged()
    {
        var amt = _damagable.GetCurrentHealth() / 100;
        fillImage.DOFillAmount(amt,0.5f);
    }

    private void OnDestroy()
    {
        _damagable.OnDamaged -= OnDamaged;
    }
}
