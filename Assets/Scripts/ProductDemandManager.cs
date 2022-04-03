using System;
using System.Collections.Generic;
using BML.Scripts.Utils;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

namespace BML.Scripts
{
    public class ProductDemandManager : MonoBehaviour
    {
        [SerializeField] private List<RandomUtils.WeightPair<TaskManager.Task>> productWeighting;

        public TaskManager.Task GetNextProduct()
        {
            return RandomUtils.RandomWithWeights(productWeighting);
        }

        private TaskManager.Task UnweightedRandomProduct()
        {
            int randomProduct = Random.Range((int) TaskManager.Task.GrabProductA, (int) TaskManager.Task.GrabProductF + 1);
            TaskManager.Task randomProductTask = (TaskManager.Task) Enum.ToObject(typeof(TaskManager.Task), randomProduct);
            return randomProductTask;
        }
    }
}