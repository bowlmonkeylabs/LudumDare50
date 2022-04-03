using BML.ScriptableObjectCore.Scripts.Events;
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
        
        public UnityEvent OnDialogueFinished;

        public void StartDialogue()
        {
            DialogueInfo dialogueInfo = new DialogueInfo()
            {
                Dialogue = Dialogue,
                ConversationName = ConversationName,
                OnDialogueFinished = OnDialogueFinished
            };
            OnStartConversation.Raise(dialogueInfo);
        }
    }
}