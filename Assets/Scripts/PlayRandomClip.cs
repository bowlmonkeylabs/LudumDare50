using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    public class PlayRandomClip : MonoBehaviour
    {
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private List<AudioClip> clipList;
        
        private int lastIndex;

        public void Play()
        {
            int randomIndex = Random.Range(0, clipList.Count);
            int cycleAttempts = 0;

            //Try playing diff audio
            while (cycleAttempts < 3 && randomIndex == lastIndex)
            {
                randomIndex = Random.Range(0, clipList.Count);
                cycleAttempts++;
            }
            
            audioSource.clip = clipList[randomIndex];
            audioSource.Play();

            lastIndex = randomIndex;
        }
    }
}