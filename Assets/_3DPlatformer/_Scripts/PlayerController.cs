using Cinemachine;
using KBCore.Refs;
using UnityEngine;

namespace Platformer
{
    // PlayerController class handles player movement, rotation, and interaction with the camera
    public class PlayerController : ValidatedMonoBehaviour
    {
        // Editor header for better organization of serialized fields
        [Header(" Elements ")] 
        // Reference to the CharacterController component attached to the player
        [SerializeField, Self] private CharacterController _characterController;
        // Reference to the Animator component of the player
        [SerializeField, Child] private Animator _animator;
        // Reference to the Cinemachine free-look camera
        [SerializeField, Anywhere] private CinemachineFreeLook _freeLookCamera;
        // Reference to the InputReader component for handling player input
        [SerializeField, Anywhere] private InputReader _input;
        // Reference to the Transform component of the player
        [SerializeField, Self] private Transform _transform;

        [Header(" Settings ")]
        // Movements speed of the player
        [SerializeField] private float _moveSpeed = 6f;
        // Rotation speed of the player
        [SerializeField] private float _rotationSpeed = 15f;
        // Time to smooth the speed changes
        [SerializeField] private float _smoothTime = 0.2f;

        // Reference to the main camera's transform
        private Transform _mainCam;
        // Current speed of the player
        private float _currentSpeed;
        // Velocity used for smoothing speed
        private float _velocity;
        // Constant value for zero
        private const float ZERO = 0;
        // Hash for the "Speed" parameter in the animator
        private static readonly int Speed = Animator.StringToHash("Speed");

        // Initialization function called when the script instance is being loaded
        private void Awake()
        {
            // Get the main camera transform
            _mainCam = Camera.main?.transform;
            // Set the Cinemachine camera's LookAt and Follow targets to the player's transform
            _freeLookCamera.LookAt = _transform;
            _freeLookCamera.Follow = _transform;
            // Adjust camera position to avoid initial jerk
            _freeLookCamera.OnTargetObjectWarped(transform, transform.position - _freeLookCamera.transform.position - Vector3.one);
        }

        // Start is called before the first frame update
        private void Start() => _input.EnablePlayerActions();

        // Update is called once per frame
        private void Update()
        {
            // Handle player movement based on input
            HandleMovementInput();
            // Update the animator with the current speed
            UpdateAnimator();
        }

        // Updates the animator with the current speed
        private void UpdateAnimator() => _animator.SetFloat(Speed, _currentSpeed);

        // Handles player movement input
        private void HandleMovementInput()
        {
            var movementDirection = new Vector3(_input.Direction.x, 0, _input.Direction.y);

            // Adjust direction according to the camera's rotation
            var adjustedDirection = Quaternion.AngleAxis(_mainCam.eulerAngles.y, Vector3.up) * movementDirection;
            if (adjustedDirection.magnitude > ZERO)
            {
                // Handle player rotation and movement
                HandleRotation(adjustedDirection);
                HandleCharacterController(adjustedDirection);
                SmoothSpeed(adjustedDirection.magnitude);
            }
            else
                SmoothSpeed(ZERO);
        }

        // Moves the character controller based on the adjusted direction
        private void HandleCharacterController(Vector3 adjustedDirection)
        {
            var adjustMovement = adjustedDirection * (_moveSpeed * Time.deltaTime);
            _characterController.Move(adjustMovement);
        }

        // Rotates the player towards the target direction
        private void HandleRotation(Vector3 adjustedDirection)
        {
            var targetRotation = Quaternion.LookRotation(adjustedDirection);
            _transform.rotation = Quaternion.RotateTowards(_transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
            // Potential alternate method for rotation
            // _transform.LookAt(_transform.position + adjustedDirection); // Commented out for reference
        }

        // Smoothly updates the player's speed
        private void SmoothSpeed(float targetSpeed)
        {
            _currentSpeed = Mathf.SmoothDamp(_currentSpeed, targetSpeed, ref _velocity, _smoothTime);
        }
    }
}