using System.Collections.Generic;
using UnityEngine;

namespace BML.Scripts
{
    public class FootstepSounds : MonoBehaviour
    {
        [SerializeField] private float distToStep = 2f;
        [SerializeField] private float volumeMult = .2f;
        [SerializeField] private AudioSource audiosSource;
        [SerializeField] private List<AudioClip> footStepSounds;
        
        private Vector3 lastPos;
        private float distMoved;
 
        void Update() {
            distMoved += (lastPos - transform.position).magnitude;
            lastPos = transform.position;
 
            if(distMoved > distToStep)
            {
                int randIndex = Random.Range(0, footStepSounds.Count);
                audiosSource.PlayOneShot(footStepSounds[randIndex], volumeMult);
 
                distMoved = 0;
            }
        }
    }
}