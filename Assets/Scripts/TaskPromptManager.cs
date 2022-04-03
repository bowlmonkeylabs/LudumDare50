using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace BML.Scripts
{
    public class TaskPromptManager : SerializedMonoBehaviour
    {
        [SerializeField] private Dictionary<TaskManager.Task, GameObject> uiObjects;

        private RectTransform[] children;
        
        private void Awake()
        {
            children = this.transform.GetComponentsInChildren<RectTransform>();
        }

        public void SetPrompt(TaskManager.Task task)
        {
            foreach (var uiObject in uiObjects.Values)
            {
                uiObject.SetActive(false);
            }

            uiObjects[task].SetActive(true);
        }
        
    }
}