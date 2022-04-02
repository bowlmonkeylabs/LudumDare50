using UnityEngine;

namespace BML.Scripts
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private LayerMask interactMask;
        [SerializeField] private Transform mainCamera;
        [SerializeField] private float interactDistance = 10f;

        public void OnInteract()
        {
            RaycastHit hit;
            if (Physics.Raycast(mainCamera.position, mainCamera.forward, out hit, interactDistance, interactMask))
            {
                //Debug.Log($"Hit {hit.collider.gameObject.name}");

                InteractionReceiver interactionReceiver = hit.collider.GetComponent<InteractionReceiver>();
                if (interactionReceiver == null) return;
                
                interactionReceiver.ReceiveInteraction();
            }
        }
    }
}