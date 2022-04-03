using System;
using System.Collections;
using System.Collections.Generic;
using BML.ScriptableObjectCore.Scripts.Variables;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

namespace BML.Scripts
{
    public class UiQuotaController : MonoBehaviour
    {
        [SerializeField] private IntReference quotaCount;
        [SerializeField] private IntReference quotaGoal;
        [SerializeField] private TMP_Text textQuotaCount;
        [SerializeField] private TMP_Text textQuotaGoal;

        private void Awake()
        {
            quotaCount.Subscribe(UpdateCount);
            quotaGoal.Subscribe(UpdateGoal);
            UpdateCount();
            UpdateGoal();
        }

        private void OnDestroy()
        {
            quotaCount.Unsubscribe(UpdateCount);
            quotaGoal.Unsubscribe(UpdateGoal);
        }

        private void UpdateCount()
        {
            textQuotaCount.text = quotaCount.Value.ToString();
        }
        
        private void UpdateGoal()
        {
            textQuotaGoal.text = quotaGoal.Value.ToString();
        }
    }
}
