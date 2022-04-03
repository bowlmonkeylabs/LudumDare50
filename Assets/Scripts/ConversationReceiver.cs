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
            DialogueInfo dialogueInfo = (DialogueInfo) nextDialogueInfo;

            currentOnFinishDialogueEvent = dialogueInfo.OnDialogueFinished;
            handler.dialogue = dialogueInfo.Dialogue;
            handler.SetConversation(dialogueInfo.ConversationName);
            UpdateDialogueText();
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
            IsConversationActive.Value = false;
        }

        private void UpdateDialogueText()
        {
            // Generate choices if a choice, otherwise display the message
            if (handler.currentMessageInfo.Type == QD_NodeType.Message)
            {
                QD_Message message = handler.GetMessage();
                DialogueText.text = message.MessageText;
                SpeakerText.text = message.SpeakerName;
            }
        }
    }
}