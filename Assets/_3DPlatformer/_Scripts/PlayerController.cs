using System;
using System.Collections.Generic;
using Cinemachine;
using KBCore.Refs;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.PlayerLoop;
using Utilities;
using Timer = Utilities.Timer;

namespace Platformer
{
    // PlayerController class handles player movement, rotation, and interaction with the camera
    public class PlayerController : ValidatedMonoBehaviour
    {
        // Editor header for better organization of serialized fields
        [Header(" Elements ")] 
        // Reference to the CharacterController component attached to the player
        [SerializeField, Self] private Rigidbody RB;
        // Reference to the Animator component of the player
        [SerializeField, Child] private Animator _animator;
        // Reference to the Cinemachine free-look camera
        [SerializeField, Anywhere] private CinemachineFreeLook _freeLookCamera;
        // Reference to the InputReader component for handling player input
        [SerializeField, Anywhere] private InputReader _input;
        // Reference to the Transform component of the player
        [SerializeField, Self] private Transform _transform;
        [SerializeField, Self] private GroundCheck _groundCheck;
        [Header("Movement Settings ")]
        // Movements speed of the player
        [SerializeField] private float _moveSpeed = 6f;
        // Rotation speed of the player
        [SerializeField] private float _rotationSpeed = 15f;
        // Time to smooth the speed changes
        [SerializeField] private float _smoothTime = 0.2f;

        [Header("Jump Settings")] [SerializeField]
        private float _jumpMaxHeight;
        [SerializeField]
        private float _jumpTimeToApex;
        [SerializeField]
        private float _gravilityMultiplier = 2f;
        [SerializeField] private float _jumpCooldown;
        [SerializeField] private float _jumpForce; 
        private Transform _mainCam;
        private Vector3 _movement;
        private float _currentSpeed;
        private float _velocity;
        
        private float _jumpVelocity;
        
        private List<Timer> _timers;
        private CooldownTimer _jumpCooldownTimer;
        private CooldownTimer _jumpTimer;
       
       

        private const float ZERO = 0;
        private static readonly int Speed = Animator.StringToHash("Speed");
        private void Awake()
        {
            SetupCamera();
            ConfigurePhysics();
            InitializeJumpTimers();
        }

        private void SetupCamera()
        {
            _mainCam = Camera.main?.transform;
            _freeLookCamera.LookAt = _transform;
            _freeLookCamera.Follow = _transform;
            _freeLookCamera.OnTargetObjectWarped(transform, 
                transform.position - _freeLookCamera.transform.position - Vector3.one);
        }

        private void ConfigurePhysics()
        {
            RB.freezeRotation = true;
        }

        private void InitializeJumpTimers()
        {
            _jumpTimer = new CooldownTimer(_jumpTimeToApex);
            _jumpCooldownTimer = new CooldownTimer(_jumpCooldown);
            _timers = new List<Timer>(2){_jumpTimer,_jumpCooldownTimer};

            _jumpTimer.OnTimeStop += () => _jumpCooldownTimer.Start();
        }

        private void Start() => _input.EnablePlayerActions();

        private void OnEnable()
        {
            _input.Jump += OnJump;
        }

        private void OnDisable()
        {
            _input.Jump -= OnJump;
        }
        private void OnJump(bool perform)
        {
            if (perform && !_jumpCooldownTimer.IsRunning && _groundCheck.IsGrounded)
            {
                _jumpTimer.Start();
            }
            else if(!perform && _jumpCooldownTimer.IsRunning)
            {
                _jumpTimer.Stop();
            }
        }

        private void Update()
        {
            _movement = new Vector3(_input.Direction.x, 0,_input.Direction.y);
            // Update the animator with the current speed
            UpdateAnimator();
            UpdateTimer();
        }

        private void FixedUpdate()
        {
            // Handle player movement based on input
            HandleMovement();
            HandleJump();
        }

        private void UpdateTimer()
        {
            foreach (var timer in _timers)
            {
                timer.Tick(Time.deltaTime);
            }
        }
        private void HandleJump()
        {
            if (_groundCheck.IsGrounded && !_jumpTimer.IsRunning)
            {
                _jumpVelocity = ZERO;
                _jumpTimer.Stop();
            }

            if (_jumpTimer.IsRunning)
            {
                float launchPoint = 0.9f;
                if (_jumpTimer.Progress > launchPoint)
                { 
                       _jumpVelocity =  Mathf.Sqrt(2*_jumpMaxHeight*Mathf.Abs(Physics.gravity.y));    
                }
                else
                {
                       _jumpVelocity +=(1-_jumpTimer.Progress) * _jumpForce*Time.deltaTime;
                }
            }
            else
            {
                _jumpVelocity += Physics.gravity.y * _gravilityMultiplier * Time.deltaTime;
            }
            RB.velocity = new Vector3(RB.velocity.x, _jumpVelocity, RB.velocity.z);
        }
        
        private void UpdateAnimator() => _animator.SetFloat(Speed, _currentSpeed);
        private void HandleMovement()
        {
            // Adjust direction according to the camera's rotation
            var adjustedDirection = Quaternion.AngleAxis(_mainCam.eulerAngles.y, Vector3.up) * _movement;
            if (adjustedDirection.magnitude > ZERO)
            {
                // Handle player rotation and movement
                HandleRotation(adjustedDirection);
                HandleHorizontalMovement(adjustedDirection);
                SmoothSpeed(adjustedDirection.magnitude);
            }
            else
            {
                SmoothSpeed(ZERO);
                RB.velocity = new Vector3(ZERO, RB.velocity.y, ZERO);
            }
        }

        private void HandleHorizontalMovement(Vector3 adjustedDirection)
        {
            var Velocity = adjustedDirection * (_moveSpeed * Time.fixedDeltaTime);
            RB.velocity = new Vector3(Velocity.x, RB.velocity.y, Velocity.z);
        }
        private void HandleRotation(Vector3 adjustedDirection)
        {
            var targetRotation = Quaternion.LookRotation(adjustedDirection);
            _transform.rotation = Quaternion.RotateTowards(_transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
        }

        private void SmoothSpeed(float targetSpeed)
        {
            _currentSpeed = Mathf.SmoothDamp(_currentSpeed, targetSpeed, ref _velocity, _smoothTime);
        }

        
    }
}