using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class MoveOverTime : MonoBehaviour
    {
        private float moveSpeed;
        private Vector3 direction;

        public void SetMove(float speed, Vector3 dir)
        {
            moveSpeed = speed;
            direction = dir;
        }

        private void Update()
        {
            transform.position += moveSpeed * direction * Time.deltaTime;
        }
    }
}