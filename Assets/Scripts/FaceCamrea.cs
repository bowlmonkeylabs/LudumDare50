using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class FaceCamrea : MonoBehaviour
    {
        private void Update()
        {
            Vector3 dirToCam = (Camera.main.transform.position - transform.position).normalized;
            transform.rotation = Quaternion.LookRotation(dirToCam);
        }
    }
}