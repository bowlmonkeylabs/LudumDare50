using BML.ScriptableObjectCore.Scripts.Events;
using BML.ScriptableObjectCore.Scripts.Variables;
using QuantumTek.QuantumDialogue;
using UnityEngine;
using UnityEngine.Events;

namespace BML.Scripts
{
    public struct DialogueInfo
    {
        public QD_Dialogue Dialogue;
        public string ConversationName;
        public UnityEvent OnDialogueFinished;
    }

    public class StartConversation : MonoBehaviour
    {
        [SerializeField] private DynamicGameEvent OnStartConversation;
        [SerializeField] private QD_Dialogue Dialogue;
        [SerializeField] private string ConversationName = "Meet with Bob";
        [SerializeField] private IntReference CurrentDay;
        [SerializeField] private bool AppendDayIndex;

        public UnityEvent OnDialogueFinished;

        public void StartDialogue()
        {
            DialogueInfo dialogueInfo = new DialogueInfo()
            {
                Dialogue = Dialogue,
                ConversationName = ConversationName,
                OnDialogueFinished = OnDialogueFinished
            };

            if (AppendDayIndex) dialogueInfo.ConversationName += CurrentDay.Value;
            OnStartConversation.Raise(dialogueInfo);
        }
    }
}