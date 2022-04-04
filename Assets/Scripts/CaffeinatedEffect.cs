using System;
using BML.ScriptableObjectCore.Scripts.Variables;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace DefaultNamespace
{
    public class CaffeinatedEffect : MonoBehaviour
    {
        [SerializeField] private BoolReference IsCaffeinated;
        [SerializeField] private PostProcessVolume volume;

        private void Update()
        {
            if (IsCaffeinated.Value)
            {
                volume.weight = 1f;
            }
            else
            {
                volume.weight = 0f;
            }
        }
    }
}