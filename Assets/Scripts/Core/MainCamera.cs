using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RTSZombie
{
    public class MainCamera : MonoBehaviour
    {
        [HideInInspector] public float movementSpeed;

        public Vector3 cameraXDirection;

        public Vector3 cameraYDirection;

        private void Start()
        {
            movementSpeed = RZGlobalConfig.Instance.cameraMovementSpeed;

            Vector3 camForward = transform.forward;
            cameraYDirection = new Vector3(camForward.x, 0f, camForward.z).normalized;
            Vector3 camRight = transform.right;
            cameraXDirection = new Vector3(camRight.x, 0f, camRight.z).normalized;
        }

        public void MoveCamera(Vector2 screenDirection)
        {
            Vector3 cameraDirection = (cameraXDirection * screenDirection.x + cameraYDirection * screenDirection.y).normalized;
            transform.position += cameraDirection * movementSpeed * Time.deltaTime;
        }
    }



}
