using System;
using BML.ScriptableObjectCore.Scripts.Events;
using BML.ScriptableObjectCore.Scripts.Variables;
using StarterAssets;
using TMPro;
using UnityEngine;

namespace BML.Scripts
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private FirstPersonController firstPersonController;
        [SerializeField] private BoolReference isRoundStarted;
        [SerializeField] private BoolReference IsConversationActive;
        [SerializeField] private BoolReference IsHoverTextOpen;
        [SerializeField] private BoolReference IsDayTransitioning;
        [SerializeField] private BoolReference HasPissBottle;
        [SerializeField] private FloatReference currentPissAmount;
        [SerializeField] private FloatReference maxPissAmount;
        [SerializeField] private FloatReference rateOfPissPerSecond;
        [SerializeField] private FloatReference caffeineSpeedMult;
        [SerializeField] private FloatReference caffeineIncreasePissAmount;
        [SerializeField] private FloatReference maxPissBottleMult;
        [SerializeField] private TimerReference caffeineTimer;
        [SerializeField] private TimerReference DayTimer;
        [SerializeField] private LayerMask interactMask;
        [SerializeField] private Transform mainCamera;
        [SerializeField] private float interactDistance = 10f;
        [SerializeField] private float hoverDistance = 1f;
        [SerializeField] private GameEvent onPissYourself;
        [SerializeField] private GameEvent onConsumeCaffeine;
        [SerializeField] private GameEvent onContinueDialogue;
        [SerializeField] private GameEvent onUnHover;
        [SerializeField] private GameEvent onGetPissBottle;
        [SerializeField] private TMP_Text HoverText;

        private bool havePissedYourself;
        private bool isCaffeinated;
        private float originalMoveSpeed;

        private void Awake()
        {
            onConsumeCaffeine.Subscribe(TryConsumeCaffeine);
            caffeineTimer.SubscribeFinished(DisableCaffeine);
            originalMoveSpeed = firstPersonController.MoveSpeed;
            currentPissAmount.Value = 0f;
            isCaffeinated = false;
            onGetPissBottle.Subscribe(OnGetPissBottle);
            maxPissAmount.Reset();
        }

        private void OnDisable()
        {
            onConsumeCaffeine.Unsubscribe(TryConsumeCaffeine);
            caffeineTimer.UnsubscribeFinished(DisableCaffeine);
        }

        private void Update()
        {
            HandleHover();
            caffeineTimer.UpdateTime();

            if (isRoundStarted.Value && !DayTimer.IsFinished)
            {
                //Handle Piss Meter
                if (!havePissedYourself)
                    currentPissAmount.Value += rateOfPissPerSecond.Value * Time.deltaTime;

                if (!havePissedYourself && currentPissAmount.Value > maxPissAmount.Value)
                {
                    onPissYourself.Raise();
                    havePissedYourself = true;
                    currentPissAmount.Value = maxPissAmount.Value;
                }
            }
        }

        public void OnInteract()
        {
            //Continue dialogue if currently in conversation
            if (IsConversationActive.Value || IsDayTransitioning.Value)
            {
                onContinueDialogue.Raise();
                return;
            }

            RaycastHit hit;
            if (Physics.Raycast(mainCamera.position, mainCamera.forward, out hit, interactDistance, interactMask))
            {
                //Debug.Log($"Hit {hit.collider.gameObject.name}");

                InteractionReceiver interactionReceiver = hit.collider.GetComponent<InteractionReceiver>();
                if (interactionReceiver == null) return;

                interactionReceiver.ReceiveInteraction();
            }
        }

        private void HandleHover()
        {
            RaycastHit hit;
            if (Physics.Raycast(mainCamera.position, mainCamera.forward, out hit, interactDistance, interactMask))
            {
                InteractionReceiver interactionReceiver = hit.collider.GetComponent<InteractionReceiver>();
                if (interactionReceiver == null) return;

                HoverText.text = interactionReceiver.HoverText;
            }
            else
            {
                HoverText.text = "";
            }
        }

        private void CheckHover()
        {
            RaycastHit hit;
            if (Physics.Raycast(mainCamera.position, mainCamera.forward, out hit, hoverDistance, interactMask))
            {
                HoverReceiver hoverReceiver = hit.collider.GetComponent<HoverReceiver>();
                if (hoverReceiver == null)
                {
                    if (IsHoverTextOpen.Value)
                    {
                        // onUnHover.Raise();
                    }
                    return;
                }

                // if (!IsHoverTextOpen.Value) hoverReceiver.ReceiveHover();
            }
            else if (IsHoverTextOpen.Value)
            {
                // onUnHover.Raise();
            }
        }

        private void TryConsumeCaffeine()
        {
            if (isCaffeinated || !isRoundStarted.Value) return;

            isCaffeinated = true;
            caffeineTimer.RestartTimer();
            firstPersonController.MoveSpeed = originalMoveSpeed * caffeineSpeedMult.Value;
            currentPissAmount.Value += caffeineIncreasePissAmount.Value;
        }

        private void DisableCaffeine()
        {
            firstPersonController.MoveSpeed = originalMoveSpeed;
            isCaffeinated = false;
        }

        private void OnGetPissBottle()
        {
            HasPissBottle.Value = true;
            maxPissAmount.Value *= maxPissBottleMult.Value;
        }
    }
}