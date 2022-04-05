using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BML.ScriptableObjectCore.Scripts.Variables;
using Sirenix.OdinInspector;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

namespace BML.Scripts.UI
{
    public class UiReadFromSlider : MonoBehaviour
    {
        [SerializeField] private Slider slider;
        [SerializeField] private FloatVariable outValue;

        private void Awake()
        {
            outValue.Subscribe(UpdateSlider);
            UpdateSlider();
        }
        
        private void OnDestroy()
        {
            outValue.Unsubscribe(UpdateSlider);
        }

        public void ReadValue()
        {
            outValue.Value = slider.value;
        }

        public void UpdateSlider()
        {
            slider.value = outValue.Value;
        }
    }
}
