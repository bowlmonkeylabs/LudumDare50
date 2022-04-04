using System;
using System.Collections.Generic;
using BML.ScriptableObjectCore.Scripts.Variables;
using UnityEngine;
using UnityEngine.Events;

namespace DefaultNamespace
{
    public class FloatThresholdEvents : MonoBehaviour
    {
        public class ThresholdEvent
        {
            public float Threshold;
            public UnityEvent Event;
        }

        [SerializeField] private FloatReference value;
        [SerializeField] private List<ThresholdEvent> thresholdEvents;

        private float prevValue;

        private void Awake()
        {
            value.Subscribe(TryFireEvent);
            prevValue = value.Value;
        }

        private void OnDestroy()
        {
            value.Unsubscribe(TryFireEvent);
        }

        private void TryFireEvent()
        {
            foreach (var thresholdEvent in thresholdEvents)
            {
                if (prevValue < thresholdEvent.Threshold
                    && thresholdEvent.Threshold <= value.Value)
                {
                    thresholdEvent.Event.Invoke();
                }
            }

            prevValue = value.Value;
        }
    }
}