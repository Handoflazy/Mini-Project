using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace Platformer.Advanced
{
    public class CameraController : MonoBehaviour
    {
        #region  Field
        [Header("Settings")]
        float currentXAngle;
        float currentYAngle;

        [Range(0, 90f)] public float upperVerticalLimit = 35f;
        [Range(0,90f)] public float lowerVerticalLimit = 35f;

        public float cameraSpeed = 50f;
        public bool smoothCameraRotation;
        [Range(0,50f)] public float cameraSmoothingFactor = 25f;
        Transform tr;
        Camera cam;
        [SerializeField,Required] InputReader input;
        private bool _isRMBPressed;
        // Lock to temporarily disable camera movement
        private bool _cameraMovementLock;
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

        private void OnEnable()
        {
            input.CameraMoveEvent += OnCameraMoveEvent;
            input.EnableMouseControlCamera += OnEnableMouseControlCamera;
            input.DisableMouseControlCamera += OnDisableMouseControlCamera;
        }

        private void OnDisable()
        {
            input.CameraMoveEvent -= OnCameraMoveEvent;
            input.EnableMouseControlCamera -= OnEnableMouseControlCamera;
            input.DisableMouseControlCamera -= OnDisableMouseControlCamera;
        }

        private void OnDisableMouseControlCamera()
        {
            _isRMBPressed = false;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            
        }

        private IEnumerator DisableMouseForFrame()
        {
            _cameraMovementLock = true;
            yield return new WaitForEndOfFrame();
            _cameraMovementLock = false;
        }

        private void OnEnableMouseControlCamera()
        {
            _isRMBPressed = true;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            StartCoroutine(DisableMouseForFrame());
        }


        private void OnCameraMoveEvent(Vector2 cameraMovement, bool isDeviceMouse)
        {
            if (_cameraMovementLock)
                return;
            if (isDeviceMouse && !_isRMBPressed) return;
          
            var deviceMultiplier = isDeviceMouse ? Time.fixedDeltaTime : Time.deltaTime;
            RotateCamera(cameraMovement.x*deviceMultiplier, cameraMovement.y*deviceMultiplier);
            
        }
        

        private void RotateCamera(float horizontalInput, float verticalInput)
        {
            /*if (smoothCameraRotation)
            {
                horizontalInput = Mathf.Lerp(0,horizontalInput, Time.deltaTime * cameraSmoothingFactor);
                verticalInput = Mathf.Lerp(0,verticalInput, Time.deltaTime * cameraSmoothingFactor);
            }*/
            /*currentXAngle += verticalInput * Time.deltaTime * cameraSpeed;
            currentYAngle += horizontalInput * Time.deltaTime * cameraSpeed;*/
            currentXAngle += verticalInput* cameraSpeed;
            currentYAngle += horizontalInput  * cameraSpeed;
            
            currentXAngle = Mathf.Clamp(currentXAngle, -lowerVerticalLimit, upperVerticalLimit);
            tr.localRotation = Quaternion.Euler(currentXAngle, currentYAngle, 0);
        }
    }
}
