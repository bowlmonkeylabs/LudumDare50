using System;
using System.Collections;
using System.Collections.Generic;
using BML.ScriptableObjectCore.Scripts.Variables;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

namespace BML.Scripts
{
    public class UiPissGaugeController : MonoBehaviour
    {
        [SerializeField] private FloatReference valueCurrent;
        [SerializeField] private FloatReference valueMax;
        [SerializeField] private Image fillImage;
        [SerializeField] private Gradient fillColor;

        private void Awake()
        {
            valueCurrent.Subscribe(UpdateFill);
            valueMax.Subscribe(UpdateFill);
        }

        private void Start()
        {
            UpdateFill();
        }

        private void OnDestroy()
        {
            valueCurrent.Unsubscribe(UpdateFill);
            valueMax.Unsubscribe(UpdateFill);
        }

        private void UpdateFill()
        {
            var fillPct = valueCurrent.Value / valueMax.Value;
            fillImage.fillAmount = fillPct;
            fillImage.color = fillColor.Evaluate(fillPct);
        }
    }
}
