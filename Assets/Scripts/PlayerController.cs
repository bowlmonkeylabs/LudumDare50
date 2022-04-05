using System;
using BML.ScriptableObjectCore.Scripts.Events;
using BML.ScriptableObjectCore.Scripts.Variables;
using StarterAssets;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BML.Scripts
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private FirstPersonController firstPersonController;
        [SerializeField] private float highPissPercent = .75f;
        [SerializeField] private BoolReference isRoundStarted;
        [SerializeField] private BoolReference IsConversationActive;
        [SerializeField] private BoolReference IsHoverTextOpen;
        [SerializeField] private BoolReference IsDayTransitioning;
        [SerializeField] private BoolReference HasPissBottle;
        [SerializeField] private BoolReference IsCaffeinated;
        [SerializeField] private FloatReference currentPissAmount;
        [SerializeField] private FloatReference startingPissAmount;
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
        [SerializeField] private GameEvent OnPissHigh;
        [SerializeField] private TMP_Text HoverText;

        private bool havePissedYourself;

        private float originalMoveSpeed;
        private bool isPissHigh;

        private void Awake()
        {
            onConsumeCaffeine.Subscribe(TryConsumeCaffeine);
            caffeineTimer.SubscribeFinished(DisableCaffeine);
            originalMoveSpeed = firstPersonController.MoveSpeed;
            currentPissAmount.Value = 0f;
            IsCaffeinated.Value = false;
            onGetPissBottle.Subscribe(OnGetPissBottle);

            if (SceneManager.GetActiveScene () == SceneManager.GetSceneByName ("BreakRoom")) 
            {
                maxPissAmount.Reset();
                HasPissBottle.Value = false;
            }
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

            var percentPiss = currentPissAmount.Value / maxPissAmount.Value;
            if (!isPissHigh && percentPiss > highPissPercent)
            {
                OnPissHigh.Raise();
                isPissHigh = true;
            }
            if (isPissHigh && percentPiss <= highPissPercent)
            {
                isPissHigh = false;
            }
        }

        public void OnInteract()
        {
            if (IsDayTransitioning.Value) return;

            //Continue dialogue if currently in conversation
            if (IsConversationActive.Value)
            {
                onContinueDialogue.Raise();
                return;
            }

            RaycastHit hit;
            if (Physics.Raycast(mainCamera.position, mainCamera.forward, out hit, interactDistance, interactMask, QueryTriggerInteraction.Collide))
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
            if (Physics.Raycast(mainCamera.position, mainCamera.forward, out hit, interactDistance, interactMask, QueryTriggerInteraction.Collide))
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
            IsCaffeinated.Value = true;
            caffeineTimer.RestartTimer();
            firstPersonController.MoveSpeed = originalMoveSpeed * caffeineSpeedMult.Value;
            if (isRoundStarted.Value) currentPissAmount.Value += caffeineIncreasePissAmount.Value;
        }

        private void DisableCaffeine()
        {
            firstPersonController.MoveSpeed = originalMoveSpeed;
            IsCaffeinated.Value = false;
        }

        private void OnGetPissBottle()
        {
            HasPissBottle.Value = true;
            maxPissAmount.Value = startingPissAmount.Value * maxPissBottleMult.Value;
        }
    }
}