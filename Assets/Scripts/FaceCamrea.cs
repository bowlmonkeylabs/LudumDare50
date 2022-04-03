using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class FaceCamrea : MonoBehaviour
    {
        public Camera MainCamera { get; set; }
        public bool GrabMainCameraOnStart = true;
        public Vector3 OffsetDirection = Vector3.back;
        public Vector3 Up = Vector3.up;
        public Vector3 AxisMask = new Vector3(1, 0, 1);
        
        protected void Awake()
        {
            if (GrabMainCameraOnStart == true)
            {
                GrabMainCamera();
            }
        }
        
        private void Update()
        {
            Vector3 dirToCam = (Camera.main.transform.position - transform.position).normalized;
            transform.rotation = Quaternion.LookRotation(dirToCam);
            var lookAtOffset = Vector3.Scale(MainCamera.transform.rotation * OffsetDirection, AxisMask);
            transform.LookAt(transform.position + lookAtOffset, MainCamera.transform.rotation * Up);
        }
        
        /// <summary>
        /// Grabs the main camera.
        /// </summary>
        protected virtual void GrabMainCamera()
        {
            MainCamera = Camera.main;
        }
    }
}