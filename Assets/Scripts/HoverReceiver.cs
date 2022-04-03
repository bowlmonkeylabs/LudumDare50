using BML.ScriptableObjectCore.Scripts.Variables;
using BML.ScriptableObjectCore.Scripts.Events;
using UnityEngine;

namespace BML
{
    public class HoverReceiver : MonoBehaviour
    {
        [SerializeField] private StringReference hoverText;
        [SerializeField] private DynamicGameEvent OnShowHoverText;

        public void ReceiveHover()
        {
            OnShowHoverText.Raise(hoverText.Value);
        }
    }
}

