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

        public void ReadValue()
        {
            outValue.Value = slider.value;
        }
    }
}
