using System;
using BML.ScriptableObjectCore.Scripts.Events;
using BML.ScriptableObjectCore.Scripts.Variables;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

namespace BML.Scripts
{
    public class TaskManager : MonoBehaviour
    {
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
        [SerializeField] private GameEvent OnTalkToSupervisor;
        [SerializeField] private GameEvent OnPissYourself;
        
        [Title("TMP Text References")]
        [SerializeField] private TMP_Text CurrentTaskText;
        [SerializeField] private TMP_Text TimeLeftText;
        [SerializeField] private TMP_Text QuotaText;
        [SerializeField] private TMP_Text BoxesDepositedText;
        [SerializeField] private TMP_Text PissAmountText;

        [Title("Task Text")]
        [SerializeField] private String GrabBoxText = "Grab a Box";
        [SerializeField] private String GrabProductText = "Grab Product ";
        [SerializeField] private String DepositBoxText = "Deposit the Box";
        [SerializeField] private String TalkToSupervisorText = "Talk to Supervisor";
        
        [Title("Other UI Text")]
        [SerializeField] private String TimeLeftTextPrefix = "Time: ";
        [SerializeField] private String QuotaTextPrefix = "Quota: ";
        [SerializeField] private String BoxesDepositedTextPrefix = "Boxes Deposited: ";
        [SerializeField] private String PissAmountPrefix = "Piss Amount: ";

        private Task CurrentTask = Task.GrabBox;
        private int numberOfProducts = 6;
        private int currentProductIndex;

        enum Task
        {
            GrabBox,
            GrabProduct,
            DepositBox,
            TalkToSupervisorEndGame,
            TalkToSupervisorStartGame
        }

        private void Awake()
        {
            OnGrabBox.Subscribe(GrabBox);
            OnGrabProductA.Subscribe(() => GrabProduct(0));
            OnGrabProductB.Subscribe(() => GrabProduct(1));
            OnGrabProductC.Subscribe(() => GrabProduct(2));
            OnGrabProductD.Subscribe(() => GrabProduct(3));
            OnGrabProductE.Subscribe(() => GrabProduct(4));
            OnGrabProductF.Subscribe(() => GrabProduct(5));
            OnDepositBox.Subscribe(DepositBox);
            DayTimer.SubscribeFinished(TimerComplete);
            OnPissYourself.Subscribe(PissYourself);
            OnTalkToSupervisor.Subscribe(TalkToSupervisor);
        }

        private void OnDisable()
        {
            OnGrabBox.Unsubscribe(GrabBox);
            OnGrabProductA.Unsubscribe(() => GrabProduct(0));
            OnGrabProductB.Unsubscribe(() => GrabProduct(1));
            OnGrabProductC.Unsubscribe(() => GrabProduct(2));
            OnGrabProductD.Unsubscribe(() => GrabProduct(3));
            OnGrabProductE.Unsubscribe(() => GrabProduct(4));
            OnGrabProductF.Unsubscribe(() => GrabProduct(5));
            OnDepositBox.Unsubscribe(DepositBox);
            DayTimer.UnsubscribeFinished(TimerComplete);
            OnPissYourself.Unsubscribe(PissYourself);
            OnTalkToSupervisor.Unsubscribe(TalkToSupervisor);
        }

        private void Start()
        {
            CurrentTask = Task.TalkToSupervisorStartGame;
            CurrentTaskText.text = TalkToSupervisorText;
            CurrentQuota.Value = Mathf.FloorToInt(QuotaCurve.Value.Evaluate(CurrentDay.Value));
            QuotaText.text = QuotaTextPrefix + CurrentQuota.Value;
        }

        private void Update()
        {
            DayTimer.UpdateTime();
            TimeLeftText.text = TimeLeftTextPrefix + Mathf.CeilToInt(DayTimer.RemainingTime ?? 0);
            BoxesDepositedText.text = BoxesDepositedTextPrefix + BoxesDepositedCount.Value;
            PissAmountText.text = PissAmountPrefix + Mathf.Floor(CurrentPissAmount.Value);
        }

        private void GrabBox()
        {
            if (CurrentTask != Task.GrabBox) return;
            
            CurrentTask = Task.GrabProduct;
            SelectNextProduct();
            CurrentTaskText.text = GrabProductText + currentProductIndex;
            Debug.Log("Grabbed Box");
            
        }

        private void GrabProduct(int productIndex)
        {
            if (CurrentTask != Task.GrabProduct || 
                productIndex != currentProductIndex)
                return;
            
            CurrentTask = Task.DepositBox;
            CurrentTaskText.text = DepositBoxText;
            Debug.Log("Grabbed Product");
            
        }

        private void DepositBox()
        {
            if (CurrentTask != Task.DepositBox) return;
            
            CurrentTask = Task.GrabBox;
            CurrentTaskText.text = GrabBoxText;
            BoxesDepositedCount.Value++;
            Debug.Log($"Deposited Box #{BoxesDepositedCount.Value}");
        }

        private void TimerComplete()
        {
            CurrentTask = Task.TalkToSupervisorEndGame;
            CurrentTaskText.text = TalkToSupervisorText;
        }

        private void PissYourself()
        {
            CurrentTask = Task.TalkToSupervisorEndGame;
            CurrentTaskText.text = TalkToSupervisorText;
            DayTimer.StopTimer();
        }

        private void TalkToSupervisor()
        {
            if (CurrentTask == Task.TalkToSupervisorStartGame)
            {
                //TODO: tell player what quota for the day is
                //set task
                //start timer
                CurrentTask = Task.GrabBox;
                CurrentTaskText.text = GrabBoxText;
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
                SceneManager.LoadScene(BreakRoomSceneName);
            }
        }

        private void SelectNextProduct()
        {
            currentProductIndex = Random.Range(0, numberOfProducts);
        }
    }
}