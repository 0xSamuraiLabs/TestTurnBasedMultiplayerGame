using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
[ExecuteInEditMode]
public class FillGradient : MonoBehaviour
{
    [SerializeField] private Gradient _gradient = null;
     [SerializeField] private Image _image = null;
     
        private void Awake()
        {
            _image = GetComponent<Image>();
        }
     
        private void Update()
        {
            if (_gradient != null)
            {
                _image.color = _gradient.Evaluate(_image.fillAmount);
            }
        }
}


