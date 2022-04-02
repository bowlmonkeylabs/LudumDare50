using System;
using BML.ScriptableObjectCore.Scripts.Events;
using BML.ScriptableObjectCore.Scripts.Variables;
using TMPro;
using UnityEngine;

namespace BML.Scripts
{
    public class TaskManager : MonoBehaviour
    {
        [SerializeField] private GameEvent OnGrabBox;
        [SerializeField] private GameEvent OnGrabProduct;
        [SerializeField] private GameEvent OnDepositBox;
        [SerializeField] private IntReference BoxesDepositedCount;
        [SerializeField] private TMP_Text CurrentTaskText;
        [SerializeField] private String GrabBoxText = "Grab a Box";
        [SerializeField] private String GrabProductText = "Grab the Product";
        [SerializeField] private String DepositBoxText = "Deposit the Box";

        private Task CurrentTask = Task.GrabBox;
        
        enum Task
        {
            GrabBox,
            GrabProduct,
            DepositBox
        }

        private void Awake()
        {
            OnGrabBox.Subscribe(GrabBox);
            OnGrabProduct.Subscribe(GrabProduct);
            OnDepositBox.Subscribe(DepositBox);
        }

        private void Start()
        {
            CurrentTaskText.text = GrabBoxText;
        }

        private void GrabBox()
        {
            if (CurrentTask == Task.GrabBox)
            {
                CurrentTask = Task.GrabProduct;
                CurrentTaskText.text = GrabProductText;
                Debug.Log("Grabbed Box");
            }
        }

        private void GrabProduct()
        {
            if (CurrentTask == Task.GrabProduct)
            {
                CurrentTask = Task.DepositBox;
                CurrentTaskText.text = DepositBoxText;
                Debug.Log("Grabbed Product");
            }
        }

        private void DepositBox()
        {
            if (CurrentTask == Task.DepositBox)
            {
                CurrentTask = Task.GrabBox;
                CurrentTaskText.text = GrabBoxText;
                BoxesDepositedCount.Value++;
                Debug.Log($"Deposited Box #{BoxesDepositedCount.Value}");
            }
        }
    }
}