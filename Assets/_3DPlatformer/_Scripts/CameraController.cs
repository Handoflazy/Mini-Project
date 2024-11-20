using System.Collections;
using Cinemachine;
using NaughtyAttributes;
using UnityEngine;

namespace Platformer
{
    // CameraController class handles the camera's movement and interaction based on player input
    public class CameraController : MonoBehaviour
    {
        // Group of component references for ease of editing in the inspector
        [Header("Elements")]
        // Reference to the InputReader component for capturing camera-related inputs
        [SerializeField, Required] private InputReader _input;
        // Reference to the Cinemachine FreeLook virtual camera component
        [SerializeField, Required] private CinemachineFreeLook _freeLookVCam;

        // Group of settings for camera movement behavior
        [Header("Settings")]
        // Multiplier for adjusting the camera's movement speed
        [SerializeField, Range(0.5f, 3f)] private float _speedMultiplier = 1f;

        // Variable to track if the right mouse button (RMB) is pressed
        private bool _isRMBPressed;
        // Lock to temporarily disable camera movement
        private bool _cameraMovementLock;

        // Event subscription when the object is enabled
        private void OnEnable()
        {
            _input.Look += OnLook;
            _input.EnableMouseControlCamera += OnEnableMouseControlCamera;
            _input.DisableMouseControlCamera += OnDisableMouseControlCamera;
        }

        // Event unsubscription when the object is disabled
        private void OnDisable()
        {
            _input.Look -= OnLook;
            _input.EnableMouseControlCamera -= OnEnableMouseControlCamera;
            _input.DisableMouseControlCamera -= OnDisableMouseControlCamera;
        }

        // Enables mouse control for the camera
        private void OnEnableMouseControlCamera()
        {
            _isRMBPressed = true;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            // Start a coroutine to temporarily disable mouse control for a frame
            StartCoroutine(DisableMouseForFrame());
        }

        // Disables mouse control for the camera
        private void OnDisableMouseControlCamera()
        {
            _isRMBPressed = false;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            // Reset the virtual camera input axis values
            _freeLookVCam.m_XAxis.m_InputAxisValue = 0f;
            _freeLookVCam.m_YAxis.m_InputAxisValue = 0f;
        }

        // Coroutine to disable mouse control for a single frame
        private IEnumerator DisableMouseForFrame()
        {
            _cameraMovementLock = true;
            yield return new WaitForEndOfFrame();
            _cameraMovementLock = false;
        }

        // Handles the camera movement based on input
        private void OnLook(Vector2 cameraMovement, bool isDeviceMouse)
        {
            // Skip processing if the camera movement is locked
            if (_cameraMovementLock)
                return;
            // If the input device is a mouse and RMB is not pressed, return
            if (isDeviceMouse && !_isRMBPressed) return;
            // Determine the device-specific time multiplier
            var deviceMultiplier = isDeviceMouse ? Time.fixedDeltaTime : Time.deltaTime;
            // Update the virtual camera's input axis values based on input and multiplier
            _freeLookVCam.m_XAxis.m_InputAxisValue = cameraMovement.x * deviceMultiplier * _speedMultiplier;
            _freeLookVCam.m_YAxis.m_InputAxisValue = cameraMovement.y * deviceMultiplier * _speedMultiplier;
        }
    }
}