using System.Collections.Generic;
using BML.ScriptableObjectCore.Scripts.Variables;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class GameOverTransition : MonoBehaviour
    {
        [SerializeField] private TMP_Text EndGameText;
        [SerializeField] private Image FadeToBlackImage;
        [SerializeField] private float FadeToBlackTime = 3f;
        [SerializeField] private float FadeTextTime = 2f;
        [SerializeField] private LeanTweenType FadeToBlackEase = LeanTweenType.easeOutQuart;
        [SerializeField] private LeanTweenType FadeFromBlackEase = LeanTweenType.easeInQuart;
        [SerializeField] private LeanTweenType TextFadeInEase = LeanTweenType.easeInCubic;
        
        public void StartTransiton()
        {
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
            fadeToBlackTween.setOnComplete(FadeInCurrentDayText);
        }

        private void FadeInCurrentDayText()
        {
            EndGameText.gameObject.SetActive(true);
            
            LTDescr fadeInCurrentDayTween = LeanTween.value(0f, 1f, FadeTextTime);
            fadeInCurrentDayTween.setEase(TextFadeInEase);
            fadeInCurrentDayTween.setOnUpdate(t =>
            {
                EndGameText.color = new Color(1f, 1f, 1f, t);
            });
        }
    }
}