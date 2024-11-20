/*using System;
using System.Collections.Generic;
using Cinemachine;
using Platformer._Scripts.ScriptableObject;
using Unity.VisualScripting;
using UnityEngine;
using Utilities;
using Timer = Utilities.Timer;
using Platformer.State_Machine;
using Sirenix.OdinInspector;
using IState = Platformer.State_Machine.IState;
using StateMachine = Platformer.State_Machine.StateMachine;

namespace Platformer
{
    // PlayerController class handles player movement, rotation, and interaction with the camera
    public class PlayerController : MonoBehaviour
    {
        // Editor header for better organization of serialized fields
        [Header(" Elements ")] 
        // Reference to the CharacterController component attached to the player
        [SerializeField, Required] private Rigidbody RB;
        // Reference to the Animator component of the player
        [SerializeField, Required] private Animator _animator;
        // Reference to the Cinemachine free-look camera
        [SerializeField, Required] private CinemachineFreeLook _freeLookCamera;
        // Reference to the InputReader component for handling player input
        [SerializeField, Required] private InputReader _input;
        // Reference to the Transform component of the player
        [SerializeField, Required] private Transform _transform;
        [SerializeField, Required] private GroundCheck _groundCheck;
        [SerializeField, Required] private PlayerData _data;
        
        private Transform _mainCam;
        private Vector3 _movement;
        private float _currentSpeed;
        private float _velocity;
        
        private float _jumpVelocity = 5f;
        
        private List<Timer> _timers;
        private CooldownTimer _jumpCooldownTimer;
        private CooldownTimer _jumpTimer;

        private StateMachine _stateMachine;

        private bool _isDashing = false;

        private const float ZERO = 0;
        private static readonly int Speed = Animator.StringToHash("Speed");
        private void Awake()
        {
            SetupCamera();
            ConfigurePhysics();
            InitializeJumpTimers();
            
            InitalizeStateMachine();
        }

        private void InitalizeStateMachine()
        {
            //State Machine
            /*_stateMachine = new StateMachine();
            //Declare States
            var locomotionState = new LocomotionState(this, _animator);
            var jumpState = new JumpState(this, _animator);
            var dashState = new DashState(this, _animator);
            //Define transition
            At(locomotionState,jumpState,new FuncPredicate(()=>_jumpTimer.IsRunning));
            At(locomotionState,dashState,new FuncPredicate(()=>_isDashing&&_velocity>0f));
            Any(locomotionState,new FuncPredicate(()=> !_jumpTimer.IsRunning 
                                                       && _groundCheck.IsGrounded 
                                                       && !_isDashing));
            
            _stateMachine.SetState(locomotionState);#1#
        }
        void At(IState from, IState to, IPredicate codition) => _stateMachine.AddTransition(from, to, codition);
        void Any(IState to, IPredicate codition) =>_stateMachine.AddAnyTransition(to, codition);

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
            /*_jumpTimer = new CooldownTimer(_data.JumpTimeToApex);
            _jumpCooldownTimer = new CooldownTimer(_data.JumpCooldown);
            _timers = new List<Timer>(2){_jumpTimer,_jumpCooldownTimer};
            _jumpTimer.OnTimeStart +=() =>_jumpVelocity = _data.JumpForce;
            _jumpTimer.OnTimeStop += () => _jumpCooldownTimer.Start();#1#
        }

        private void Start() => _input.EnablePlayerActions();

        private void OnEnable()
        {
            _input.Jump += OnJump;
            _input.Dash += OnDash;
        }

        private void OnDisable()
        {
            _input.Jump -= OnJump;
            _input.Dash -= OnDash;
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

        private void OnDash(bool isPerforming)
        {
            if (isPerforming && _groundCheck.IsGrounded)
            {
                _isDashing = true;
            }
            else if (!isPerforming)
            {
                _isDashing = false;
            }
        }

        private void Update()
        {
            _movement = new Vector3(_input.Direction.x, 0,_input.Direction.y);
            // Update the animator with the current speed
            UpdateAnimator();
            UpdateTimer();
            _stateMachine.Update();
        }

        private void FixedUpdate()
        {
            _stateMachine.FixedUpdate();
           
        }

        private void UpdateTimer()
        {
            foreach (var timer in _timers)
            {
                timer.Tick(Time.deltaTime);
            }
        }

        public void HandleJump()
        {
            if (_groundCheck.IsGrounded && !_jumpTimer.IsRunning)
            {
                _jumpVelocity = ZERO;
                _jumpTimer.Stop();
            }

            if (!_jumpTimer.IsRunning)
            {
               // _jumpVelocity += Physics.gravity.y * _data.GravilityMultiplier * Time.deltaTime;
            }
            RB.linearVelocity = new Vector3(RB.linearVelocity.x, _jumpVelocity, RB.linearVelocity.z);
        }
        
        private void UpdateAnimator() => _animator.SetFloat(Speed, _currentSpeed);
        public void HandleMovement()
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
                RB.linearVelocity = new Vector3(ZERO, RB.linearVelocity.y, ZERO);
            }
        }

        private void HandleHorizontalMovement(Vector3 adjustedDirection)
        {
            //var Velocity = adjustedDirection * (_data.MoveSpeed * Time.fixedDeltaTime);
            //RB.velocity = new Vector3(Velocity.x, RB.velocity.y, Velocity.z);
        }
        private void HandleRotation(Vector3 adjustedDirection)
        {
            var targetRotation = Quaternion.LookRotation(adjustedDirection);
            //_transform.rotation = Quaternion.RotateTowards(_transform.rotation, targetRotation, _data.RotationSpeed * Time.deltaTime);
        }

        private void SmoothSpeed(float targetSpeed)
        {
            //_currentSpeed = Mathf.SmoothDamp(_currentSpeed, targetSpeed, ref _velocity, _data.smoothTime);
        }


        public void HandleDash()
        {
            var adjustedDirection = Quaternion.AngleAxis(_mainCam.eulerAngles.y, Vector3.up) * _movement;
            if (!(adjustedDirection.magnitude > ZERO)) return;
            //var Velocity = adjustedDirection * (_data.DashSpeed * Time.fixedDeltaTime);
            //RB.velocity = new Vector3(Velocity.x, RB.velocity.y, Velocity.z);
            HandleRotation(adjustedDirection);

        }
    }
}*/