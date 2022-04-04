using UnityEngine;
using UnityEngine.Events;

namespace BML.Scripts
{
    public class InteractionReceiver : MonoBehaviour
    {
        [SerializeField] private string hoverText = "";
        public UnityEvent OnInteract;

        public string HoverText => hoverText;

        public void ReceiveInteraction()
        {
            OnInteract.Invoke();
        }
    }
}