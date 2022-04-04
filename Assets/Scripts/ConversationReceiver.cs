using BML.ScriptableObjectCore.Scripts.Events;
using BML.ScriptableObjectCore.Scripts.Variables;
using QuantumTek.QuantumDialogue;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace BML.Scripts
{
    public class ConversationReceiver : MonoBehaviour
    {
        [SerializeField] private QD_DialogueHandler handler;
        [SerializeField] private DynamicGameEvent OnStartConversation;
        [SerializeField] private GameEvent OnContinueDialogue;
        [SerializeField] private GameEvent OnDialogueFinished;
        [SerializeField] private TMP_Text DialogueText;
        [SerializeField] private TMP_Text SpeakerText;
        [SerializeField] private BoolReference IsConversationActive;

        [Header("Token Values")]
        [SerializeField] private IntReference CurrentQuota;
        [SerializeField] private IntReference BoxesDepositedCount;

        private UnityEvent currentOnFinishDialogueEvent;
        public void OnEnable()
        {
            OnStartConversation.Subscribe(ReceiveConversation);
            OnContinueDialogue.Subscribe(AttemptContinueDialogue);
        }

        public void OnDisable()
        {
            OnStartConversation.Unsubscribe(ReceiveConversation);
            OnContinueDialogue.Unsubscribe(AttemptContinueDialogue);
        }

        private void ReceiveConversation(System.Object prevDialogueInfo, System.Object nextDialogueInfo)
        {
            DialogueInfo dialogueInfo = (DialogueInfo)nextDialogueInfo;

            currentOnFinishDialogueEvent = dialogueInfo.OnDialogueFinished;
            handler.dialogue = dialogueInfo.Dialogue;
            handler.SetConversation(dialogueInfo.ConversationName);
            UpdateDialogueText();
            DialogueText.gameObject.SetActive(true);
            SpeakerText.gameObject.SetActive(true);
            IsConversationActive.Value = true;
        }

        private void AttemptContinueDialogue()
        {
            if (!IsConversationActive.Value) return;

            int choice = -1;
            handler.NextMessage(choice);
            UpdateDialogueText();

            // End if there is no next message
            if (handler.currentMessageInfo.ID < 0)
            {
                FinishDialogue();
            }
        }

        private void FinishDialogue()
        {
            OnDialogueFinished.Raise();
            currentOnFinishDialogueEvent.Invoke();
            DialogueText.gameObject.SetActive(false);
            SpeakerText.gameObject.SetActive(false);
            IsConversationActive.Value = false;
        }

        private void UpdateDialogueText()
        {
            // Generate choices if a choice, otherwise display the message
            if (handler.currentMessageInfo.Type == QD_NodeType.Message)
            {
                QD_Message message = handler.GetMessage();
                DialogueText.text = replaceTokens(message);
                SpeakerText.text = message.SpeakerName;
            }
        }

        private string replaceTokens(QD_Message message)
        {
            var messageText = message.MessageText;
            messageText = messageText.Replace("{{current_quota}}", "" + CurrentQuota.Value);
            messageText = messageText.Replace("{{boxes_deposited}}", "" + BoxesDepositedCount.Value);
            if (BoxesDepositedCount.Value < CurrentQuota.Value)
            {
                messageText = messageText.Replace("{{quota_status}}", "You failed to meet your quota. Do better this time.");
            }
            else
            {
                messageText = messageText.Replace("{{quota_status}}", "You met quota. Now do it again.");
            }


            return messageText;
        }
    }
}