using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace BML.Scripts
{
    public class TaskPromptManager : SerializedMonoBehaviour
    {
        [SerializeField] private List<(TaskManager.Task, GameObject)> uiObjects;

        private Dictionary<TaskManager.Task, GameObject> uiObjectsMap;

        private void Awake()
        {
            uiObjectsMap = uiObjects.ToDictionary(tuple => tuple.Item1, tuple => tuple.Item2);
        }

        public void SetPrompt(TaskManager.Task task)
        {
            foreach (var uiObject in uiObjectsMap.Values)
            {
                uiObject.SetActive(false);
            }
            
            uiObjectsMap[task].SetActive(true);
        }
        
    }
}