using System;
using System.Collections.Generic;
using BML.ScriptableObjectCore.Scripts.Variables;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class DayTransition : MonoBehaviour
    {
        [SerializeField] private string SceneToLoad = "BreakRoom";
        [SerializeField] private IntReference CurrentDay;
        [SerializeField] private BoolReference IsDayTransitioning;
        [SerializeField] private TMP_Text CurrentDayText;
        [SerializeField] private Image FadeToBlackImage;
        [SerializeField] private List<string> WeekDayList = new List<string>();
        [SerializeField] private float FadeToBlackTime = 3f;
        [SerializeField] private float FadeTextTime = 2f;
        [SerializeField] private float TextStayTime = 2f;
        [SerializeField] private LeanTweenType FadeToBlackEase = LeanTweenType.easeOutQuart;
        [SerializeField] private LeanTweenType FadeFromBlackEase = LeanTweenType.easeInQuart;
        [SerializeField] private LeanTweenType TextFadeInEase = LeanTweenType.easeInCubic;
        [SerializeField] private LeanTweenType TextFadeOutEase = LeanTweenType.easeOutCubic;

        
        //END of day
        public void StartTransiton()
        {
            IsDayTransitioning.Value = true;
            CurrentDayText.text = WeekDayList[CurrentDay.Value];
            FadeToBlack();
        }

        private void FadeToBlack()
        {
            FadeToBlackImage.gameObject.SetActive(true);
            
            LTDescr fadeToBlackTween = LeanTween.value(0f, 1f, FadeToBlackTime);
            fadeToBlackTween.setEase(FadeToBlackEase);
            fadeToBlackTween.setOnUpdate(t =>
            {
                FadeToBlackImage.color = new Color(0f, 0f, 0f, t);
            });
            fadeToBlackTween.setOnComplete(() =>
            {
                SceneManager.LoadScene(SceneToLoad);
            });
        }
        
        
        //Start of day
        public void StartFinishTransition()
        {
            IsDayTransitioning.Value = true;
            CurrentDayText.text = WeekDayList[CurrentDay.Value];
            FadeToBlackImage.gameObject.SetActive(true);

            //If not break room, just fade from black
            //Otherwise show text day of week too
            if (SceneManager.GetActiveScene().name != SceneToLoad)
                FadeFromBlack();
            else
                FadeInCurrentDayText();
        }
        
        private void FadeInCurrentDayText()
        {
            CurrentDayText.gameObject.SetActive(true);
            
            LTDescr fadeInCurrentDayTween = LeanTween.value(0f, 1f, FadeTextTime);
            fadeInCurrentDayTween.setEase(TextFadeInEase);
            fadeInCurrentDayTween.setOnUpdate(t =>
            {
                CurrentDayText.color = new Color(1f, 1f, 1f, t);
            });
            fadeInCurrentDayTween.setOnComplete(TextStay);
        }

        private void TextStay()
        {
            LeanTween.value(0f, 1f, TextStayTime).setOnComplete(FadeOutCurrentDayText);
        }
        
        private void FadeOutCurrentDayText()
        {
            LTDescr fadeOutCurrentDayTween = LeanTween.value(1f, 0f, FadeTextTime);
            fadeOutCurrentDayTween.setEase(TextFadeOutEase);
            fadeOutCurrentDayTween.setOnUpdate(t =>
            {
                CurrentDayText.color = new Color(1f, 1f, 1f, t);
            });
            fadeOutCurrentDayTween.setOnComplete(FadeFromBlack);
        }
        
        private void FadeFromBlack()
        {
            FadeToBlackImage.gameObject.SetActive(true);
            
            LTDescr fadeToBlackTween = LeanTween.value(1f, 0f, FadeToBlackTime);
            fadeToBlackTween.setEase(FadeToBlackEase);
            fadeToBlackTween.setOnUpdate(t =>
            {
                FadeToBlackImage.color = new Color(0f, 0f, 0f, t);
            });
            fadeToBlackTween.setOnComplete(() =>
            {
                IsDayTransitioning.Value = false;
            });
        }
    }
}