using System;
using BML.ScriptableObjectCore.Scripts.Events;
using BML.ScriptableObjectCore.Scripts.Variables;
using BML.Scripts.UI;
using DefaultNamespace;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

namespace BML.Scripts
{
    public class TaskManager : MonoBehaviour
    {
        [SerializeField] private float lowerTimePercent = .25f;
        
        [Title("References")]
        [SerializeField] private DayTransition dayTransition; 
        
        [Title("State Vars")]
        [SerializeField] private TimerReference DayTimer;
        [SerializeField] private IntReference CurrentQuota;
        [SerializeField] private IntReference BoxesDepositedCount;
        [SerializeField] private IntReference CurrentDay;
        [SerializeField] private FloatReference CurrentPissAmount;
        [SerializeField] private BoolReference IsRoundStarted;
        [SerializeField] private string BreakRoomSceneName = "BreakRoom";
        [SerializeField] private CurveVariable QuotaCurve;

        [Title("Task Events")]
        [SerializeField] private GameEvent OnGrabBox;
        [SerializeField] private GameEvent OnGrabProductA;
        [SerializeField] private GameEvent OnGrabProductB;
        [SerializeField] private GameEvent OnGrabProductC;
        [SerializeField] private GameEvent OnGrabProductD;
        [SerializeField] private GameEvent OnGrabProductE;
        [SerializeField] private GameEvent OnGrabProductF;
        [SerializeField] private GameEvent OnDepositBox;
        [SerializeField] private GameEvent OnDepositBoxSuccess;
        [SerializeField] private GameEvent OnTalkToSupervisor;
        [SerializeField] private GameEvent OnPissYourself;
        [SerializeField] private GameEvent OnTimerLow;
        [SerializeField] private GameEvent OnRoundOver;
        [SerializeField] private GameEvent OnGrabPlaceObject;
        [SerializeField] private GameEvent OnGameOver;

        [Title("Task UIs")]
        [SerializeField] private TaskPromptManager TaskPromptManager;

        [Title("Product Demand")]
        [SerializeField] private ProductDemandManager ProductDemandManager;

        [Title("Other UI Text")]
        [SerializeField] private TMP_Text TimeLeftText;
        [SerializeField] private TMP_Text QuotaText;
        [SerializeField] private TMP_Text BoxesDepositedText;
        [SerializeField] private TMP_Text PissAmountText;
        [SerializeField] private String TimeLeftTextPrefix = "Time: ";
        [SerializeField] private String QuotaTextPrefix = "Quota: ";
        [SerializeField] private String BoxesDepositedTextPrefix = "Boxes Deposited: ";
        [SerializeField] private String PissAmountPrefix = "Piss Amount: ";

        private Task CurrentTask = Task.GrabBox;
        private int numberOfProducts = 6;
        private int currentProductIndex;
        private bool isTimeLow;

        public enum Task
        {
            GrabBox,
            GrabProductA,
            GrabProductB,
            GrabProductC,
            GrabProductD,
            GrabProductE,
            GrabProductF,
            DepositBox,
            TalkToSupervisorEndGame,
            TalkToSupervisorStartGame
        }

        private void Awake()
        {
            OnGrabBox.Subscribe(GrabBox);
            OnGrabProductA.Subscribe(HandleGrabProductA);
            OnGrabProductB.Subscribe(HandleGrabProductB);
            OnGrabProductC.Subscribe(HandleGrabProductC);
            OnGrabProductD.Subscribe(HandleGrabProductD);
            OnGrabProductE.Subscribe(HandleGrabProductE);
            OnGrabProductF.Subscribe(HandleGrabProductF);
            OnDepositBox.Subscribe(DepositBox);
            DayTimer.SubscribeFinished(TimerComplete);
            OnPissYourself.Subscribe(PissYourself);
            OnTalkToSupervisor.Subscribe(TalkToSupervisor);
            DayTimer.ResetTimer();
            DayTimer.SubscribeFinished(() => OnRoundOver.Raise());
        }

        private void OnDisable()
        {
            OnGrabBox.Unsubscribe(GrabBox);
            OnGrabProductA.Unsubscribe(HandleGrabProductA);
            OnGrabProductB.Unsubscribe(HandleGrabProductB);
            OnGrabProductC.Unsubscribe(HandleGrabProductC);
            OnGrabProductD.Unsubscribe(HandleGrabProductD);
            OnGrabProductE.Unsubscribe(HandleGrabProductE);
            OnGrabProductF.Unsubscribe(HandleGrabProductF);
            OnDepositBox.Unsubscribe(DepositBox);
            DayTimer.UnsubscribeFinished(TimerComplete);
            OnPissYourself.Unsubscribe(PissYourself);
            OnTalkToSupervisor.Unsubscribe(TalkToSupervisor);
        }

        private void Start()
        {
            CurrentTask = Task.TalkToSupervisorStartGame;
            TaskPromptManager.SetPrompt(CurrentTask);
            CurrentQuota.Value = Mathf.FloorToInt(QuotaCurve.Value.Evaluate(CurrentDay.Value));
            QuotaText.text = QuotaTextPrefix + CurrentQuota.Value;
            BoxesDepositedCount.Value = 0;
            isTimeLow = false;
        }

        private void Update()
        {
            DayTimer.UpdateTime();
            TimeLeftText.text = TimeLeftTextPrefix + Mathf.CeilToInt(DayTimer.RemainingTime ?? 0);
            BoxesDepositedText.text = BoxesDepositedTextPrefix + BoxesDepositedCount.Value;
            PissAmountText.text = PissAmountPrefix + Mathf.Floor(CurrentPissAmount.Value);

            if (!isTimeLow && DayTimer.ElapsedTime / DayTimer.Duration > lowerTimePercent)
            {
                OnTimerLow.Raise();
                isTimeLow = true;
            }
        }

        private void GrabBox()
        {
            if (CurrentTask != Task.GrabBox) return;
            
            CurrentTask = ProductDemandManager.GetNextProduct();
            TaskPromptManager.SetPrompt(CurrentTask);
            Debug.Log("Grabbed Box");
            OnGrabPlaceObject.Raise();
        }

        private void HandleGrabProductA() => GrabProduct(Task.GrabProductA);
        private void HandleGrabProductB() => GrabProduct(Task.GrabProductB);
        private void HandleGrabProductC() => GrabProduct(Task.GrabProductC);
        private void HandleGrabProductD() => GrabProduct(Task.GrabProductD);
        private void HandleGrabProductE() => GrabProduct(Task.GrabProductE);
        private void HandleGrabProductF() => GrabProduct(Task.GrabProductF);

        private void GrabProduct(Task task)
        {
            if (CurrentTask != task)
                return;
            
            CurrentTask = Task.DepositBox;
            TaskPromptManager.SetPrompt(CurrentTask);
            
            Debug.Log("Grabbed Product");
            OnGrabPlaceObject.Raise();
        }

        private void DepositBox()
        {
            if (CurrentTask != Task.DepositBox) return;
            
            CurrentTask = Task.GrabBox;
            TaskPromptManager.SetPrompt(CurrentTask);
            BoxesDepositedCount.Value++;
            Debug.Log($"Deposited Box #{BoxesDepositedCount.Value}");
            OnDepositBoxSuccess.Raise();
            OnGrabPlaceObject.Raise();
        }

        private void TimerComplete()
        {
            CurrentTask = Task.TalkToSupervisorEndGame;
            TaskPromptManager.SetPrompt(CurrentTask);
        }

        private void PissYourself()
        {
            CurrentTask = Task.TalkToSupervisorEndGame;
            TaskPromptManager.SetPrompt(CurrentTask);
            DayTimer.StopTimer();
        }

        private void TalkToSupervisor()
        {
            if (CurrentTask == Task.TalkToSupervisorStartGame)
            {
                //Start end game if last day
                if (CurrentDay.Value >= 4)
                {
                    OnGameOver.Raise();
                    return;
                }
                
                //TODO: tell player what quota for the day is
                //set task
                //start timer
                CurrentTask = Task.GrabBox;
                TaskPromptManager.SetPrompt(CurrentTask);
                IsRoundStarted.Value = true;
                DayTimer.RestartTimer();
            }
            
            else if (CurrentTask == Task.TalkToSupervisorEndGame)
            {
                //tell player whether they met quota
                //if not final day, increment day and transition to the next
                // CurrentDay.Value++;
                // SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                //if final day, do camera turn around and set player model to robot model
                CurrentDay.Value++;
                dayTransition.StartTransiton();
            }
        }
    }
}