using System;
using System.Collections;
using System.Collections.Generic;
using BML.ScriptableObjectCore.Scripts.Variables;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

namespace BML.Scripts
{
    public class UiTimerController : MonoBehaviour
    {
        [SerializeField] private TimerReference timer;
        [SerializeField] private Image fillImage;
        [SerializeField] private Gradient fillColor;

        private void Awake()
        {
            timer.Subscribe(UpdateFill);
            UpdateFill();
        }

        private void OnDestroy()
        {
            timer.Unsubscribe(UpdateFill);
        }

        private void UpdateFill()
        {
            var timerPct = timer.ElapsedTime / timer.Duration;
            fillImage.fillAmount = timerPct;
            fillImage.color = fillColor.Evaluate(timerPct);
        }
    }
}
