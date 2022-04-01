using System;
using BML.ScriptableObjectCore.Scripts.Variables;
using Sirenix.Utilities;
using UnityEngine;

namespace BML.ScriptableObjectCore.Scripts.Variables.VariableWrappers
{
    public class SetEnabled : MonoBehaviour
    {
        [SerializeField] private BoolReference Enabled;
        [SerializeField] private MonoBehaviour[] Targets;

        private void Start()
        {
            Enabled.Subscribe(() =>
            {
                Targets.ForEach(t => t.enabled = Enabled.Value);
            });
        }
    }
}