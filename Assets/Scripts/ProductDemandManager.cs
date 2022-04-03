using System;
using System.Collections.Generic;
using BML.Scripts.Utils;
using Sirenix.OdinInspector;
using UnityEngine;

namespace BML.Scripts
{
    public class ProductDemandManager : MonoBehaviour
    {
        [SerializeField] private List<RandomUtils.WeightPair<TaskManager.Task>> productWeighting;

        public TaskManager.Task GetNextProduct()
        {
            return RandomUtils.RandomWithWeights(productWeighting);
            return TaskManager.Task.DepositBox;
        }
    }
}