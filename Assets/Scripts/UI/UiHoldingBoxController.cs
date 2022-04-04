using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BML.ScriptableObjectCore.Scripts.Variables;
using Sirenix.OdinInspector;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

namespace BML.Scripts.UI
{
    public class UiHoldingBoxController : SerializedMonoBehaviour
    {
        [SerializeField] private List<(TaskManager.Task, Image)> taskImages;

        private Dictionary<TaskManager.Task, Image> taskImagesLookup;
        private List<Image> uniqueImages;
        
        private void Awake()
        {
            taskImagesLookup = taskImages.ToDictionary(tuple => tuple.Item1, tuple => tuple.Item2);
            uniqueImages = taskImagesLookup.Values.Distinct().ToList();
        }

        public void UpdateTask(TaskManager.Task task)
        {
            foreach (var uniqueImage in uniqueImages)
            {
                uniqueImage.gameObject.SetActive(false);
            }

            taskImagesLookup.TryGetValue(task, out var image);
            if (image != null)
            {
                image.gameObject.SetActive(true);
            }
        }
    }
}
