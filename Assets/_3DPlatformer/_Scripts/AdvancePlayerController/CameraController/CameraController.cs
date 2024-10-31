using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Platformer.Advanced
{
    public class CameraController : MonoBehaviour
    {
        #region  Field

        float currentXAngle;
        float currentYAngle;

        [Range(0, 90f)] public float upperVerticalLimit = 35f;
        [Range(0,90f)] public float lowerVerticalLimit = 35f;

        public float cameraSpeed = 50f;
        public bool smoothCameraRotation;
        [Range(0,50f)] public float cameraSmoothingFactor = 25f;
        Transform tr;
        Camera cam;
        [SerializeField,Required] InputReader inputReader;

        #endregion

        public Vector3 GetUpDirection() => tr.up;
        public Vector3 GetForwardDirection() => tr.forward;

        private void Awake()
        {
            tr = transform;
            cam = GetComponentInChildren<Camera>();
             currentXAngle = tr.localRotation.eulerAngles.x;
             currentYAngle = tr.localRotation.eulerAngles.y;
        }

        private void Update()
        {
            RotateCamera(inputReader.LookDirection.x, inputReader.LookDirection.y);
        }

        private void RotateCamera(float horizontalInput, float verticalInput)
        {
            if (smoothCameraRotation)
            {
                horizontalInput = Mathf.Clamp(0,horizontalInput, Time.deltaTime * cameraSmoothingFactor);
                verticalInput = Mathf.Clamp(0,verticalInput, Time.deltaTime * cameraSmoothingFactor);
            }
            currentXAngle += verticalInput * Time.deltaTime * cameraSpeed;
            currentYAngle += horizontalInput * Time.deltaTime * cameraSpeed;
            
            currentXAngle = Mathf.Clamp(currentXAngle, -lowerVerticalLimit, upperVerticalLimit);
            tr.localRotation = Quaternion.Euler(currentXAngle, currentYAngle, 0);
        }
    }
}
