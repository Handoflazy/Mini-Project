using System;
using System.Numerics;
using AdvancedController;
using ImprovedTimers;
using Platformer.Advanced;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityUtils;
using IState = UnityUtils.StateMachine.IState;
using StateMachine = UnityUtils.StateMachine.StateMachine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

namespace Platformer.AdvancePlayerController
{
    public class PlayerController : MonoBehaviour
    {
        #region Fields
            [SerializeField,Required] InputReader inputReader;
            private Transform tr;
            private PlayerMover mover;
            
            bool jumpInputIsLocked, JumpKeyWasPressed, jumpKeyWasLetGo, jumpKeyIsPressed;

            public float MovementSpeed = 7f;
            public float AirControlRate = 2f;
            public float JumpSpeed = 10f;
            public float JumpDuration = 0.2f;
            public float AirFriction = 0.6f;
            public float GroundFiction = 100f;
            public float Gravity = 30f;
            public float SlideGravity = 5f;
            public float SlopeLimit = 30f;
            public bool useLocalMomentum;

            private StateMachine stateMachine;
            private CountdownTimer jumpTimer;
            [SerializeField, Required] private CeilingDetector ceilingDetector;

            [SerializeField] Transform cameraTransform;
            private Vector3 momentum, savedVelocity, savedMovementVelocity;
            // ex: for animation effect
            public event Action<Vector2> OnJump = delegate { };
            public event Action<Vector2> OnLand = delegate { };
                
            #endregion

            private void Awake()
            {
                tr = transform;
                mover = GetComponent<PlayerMover>();
                jumpTimer = new CountdownTimer(JumpDuration);
                SetupStateMachine();
            }

            private void Start()
            {
                inputReader.Jump += HandleJumpKeyInput;
                inputReader.EnablePlayerActions();
            }

            private void OnDisable()
            {
                inputReader.Jump -= HandleJumpKeyInput;
            }


            private void SetupStateMachine()
            {
                stateMachine = new StateMachine();
                var grounded = new GroundedState(this);
                var falling = new FallingState(this);
                var sliding = new SlidingState(this);
                var rising = new RisingState(this);
                var jumping = new JumpingState(this);
                
                At(grounded,rising,()=>IsRising());
                At(grounded,sliding,()=>mover.IsGrounded()&&IsGroundTooSteep());
                At(grounded,falling,()=>!mover.IsGrounded());
                At(grounded,jumping,()=>(JumpKeyWasPressed||jumpKeyIsPressed)&&!jumpInputIsLocked);
                
                At(falling,rising,()=>IsRising());
                At(falling,grounded,()=>mover.IsGrounded());
                At(falling,sliding,()=>mover.IsGrounded()&&IsGroundTooSteep());
                
                At(sliding, rising,()=>IsRising());
                At(sliding,falling,()=>!mover.IsGrounded());
                At(sliding,grounded,()=>mover.IsGrounded()&&!IsGroundTooSteep());
                
                At(rising,grounded,()=>mover.IsGrounded()&&!IsGroundTooSteep());
                At(rising,sliding,()=>mover.IsGrounded()&&IsGroundTooSteep());
                At(rising,falling,()=>IsFalling());
                At(rising,falling,()=>ceilingDetector!=null&& ceilingDetector.HitCeiling());
                
                
                At(jumping,rising,()=>jumpTimer.IsFinished||jumpKeyWasLetGo);
                At(jumping,falling,()=>ceilingDetector!=null&& ceilingDetector.HitCeiling());
                stateMachine.SetState(falling);
                

            }
            void At(IState from, IState to, Func<bool> codition) => stateMachine.AddTransition(from, to, codition);
            void Any(IState to, Func<bool> codition) =>stateMachine.AddAnyTransition(to, codition);
            public Vector2 GetMomentum() => useLocalMomentum?tr.localToWorldMatrix * momentum: momentum;
            
            bool IsRising()=>VectorMath.GetDotProduct(GetMomentum(), tr.up) > 0f;
            bool IsFalling()=>VectorMath.GetDotProduct(GetMomentum(), tr.up) <0f;
            bool IsGroundTooSteep()=>Vector3.Angle(mover.GetGroundNormal(), tr.up) > SlopeLimit;

            void HandleJumpKeyInput(bool isButtonPressed)
            {
                if (!jumpKeyIsPressed && isButtonPressed)
                {
                    JumpKeyWasPressed = true;
                }

                if (jumpKeyIsPressed && !isButtonPressed)
                {
                    jumpKeyWasLetGo = true;
                    jumpInputIsLocked = false;

                }
                jumpKeyIsPressed = isButtonPressed;
            }
            void ResetJumpKeys() {
                jumpKeyWasLetGo = false;
                JumpKeyWasPressed = false;
            }
            void FixedUpdate()
            {
                stateMachine.FixedUpdate();
                mover.CheckForGround();
                HandleMomentum();
                Vector3 velocity = stateMachine.CurrentState is GroundedState? CalculateMovementVelocity():Vector3.zero;
                velocity += useLocalMomentum ? tr.localToWorldMatrix * momentum : momentum;
                mover.SetExtendSensorRange(IsGrounded());
                mover.SetVelocity(velocity);
                savedVelocity = velocity;
                savedMovementVelocity = CalculateMovementVelocity();
                
                ResetJumpKeys();
                ceilingDetector.Reset();
                
               
            }

            private void Update()
            {
                stateMachine.Update();
            }

            private void HandleMomentum()
            {
                if(useLocalMomentum) momentum = tr.localToWorldMatrix * momentum; //_tf.localToWorldMatrix.MultipleVector(momentum)
                Vector3 verticalMomentum = VectorMath.ExtractDotVector(momentum, tr.up);
                Vector3 horizontalMomentum = momentum - verticalMomentum;
                
                verticalMomentum -= transform.up*(Gravity*Time.deltaTime);

                if (stateMachine.CurrentState is GroundedState &&
                    VectorMath.GetDotProduct(verticalMomentum, tr.up) < 0f)
                {
                    verticalMomentum = Vector3.zero;
                }
                if (!IsGrounded())
                {
                    AdjustHorinzontalMomentum(ref horizontalMomentum, CalculateMovementVelocity());
                }

                if (stateMachine.CurrentState is SlidingState) 
                {
                    HandleSliding(ref horizontalMomentum);
                }
                float friction = stateMachine.CurrentState is GroundedState ? GroundFiction: AirFriction;
                horizontalMomentum = Vector3.MoveTowards(horizontalMomentum,Vector3.zero,friction*Time.deltaTime);

                momentum = horizontalMomentum + verticalMomentum;
                
                if (stateMachine.CurrentState is SlidingState) {
                    momentum = Vector3.ProjectOnPlane(momentum, mover.GetGroundNormal());
                    if (VectorMath.GetDotProduct(momentum, tr.up) > 0f) {
                        momentum = VectorMath.RemoveDotVector(momentum, tr.up);
                    }
            
                    Vector3 slideDirection = Vector3.ProjectOnPlane(-tr.up, mover.GetGroundNormal()).normalized;
                    momentum += slideDirection * (SlideGravity * Time.deltaTime);
                }

   
                
                
                if(useLocalMomentum) momentum = tr.worldToLocalMatrix * momentum;
            }

            private void HandleJumping()
            {
               momentum = VectorMath.RemoveDotVector(momentum, tr.up);
               momentum += tr.up * JumpSpeed;
            }

            private void HandleSliding(ref Vector3 horizontalMomentum)
            {
                Vector3 pointDownVector = Vector3.ProjectOnPlane(mover.GetGroundNormal(), tr.up).normalized;
                Vector3 movementVelocity = CalculateMovementVelocity();
                movementVelocity = VectorMath.RemoveDotVector(movementVelocity, pointDownVector);
                horizontalMomentum += movementVelocity*Time.fixedDeltaTime; 
                
                
            }

            bool IsGrounded() => stateMachine.CurrentState is GroundedState or SlidingState;

            void AdjustHorinzontalMomentum(ref Vector3 horizontalMomentum, Vector3 movementVelocity)
            {
                if (horizontalMomentum.magnitude > MovementSpeed)
                {
                    if (VectorMath.GetDotProduct(movementVelocity, horizontalMomentum.normalized) > 0f)
                    {
                        movementVelocity = VectorMath.ExtractDotVector(movementVelocity, horizontalMomentum.normalized);
                    }
                    horizontalMomentum += movementVelocity * (AirControlRate * Time.deltaTime*0.25f);
                }
                else
                {
                    horizontalMomentum += movementVelocity * (AirControlRate * Time.deltaTime);
                    horizontalMomentum = Vector3.ClampMagnitude(horizontalMomentum, MovementSpeed);
                }
            }
            private Vector3 CalculateMovementVelocity() => CalculateMovementDirection() * MovementSpeed;
            private Vector3 CalculateMovementDirection()
            {
                Vector3 direction = cameraTransform==null ? tr.right* inputReader.Direction.x+tr.forward*inputReader.Direction.y
                : Vector3.ProjectOnPlane(cameraTransform.right, Vector3.up).normalized*inputReader.Direction.x + 
                Vector3.ProjectOnPlane(cameraTransform.forward, Vector3.up).normalized*inputReader.Direction.y ;
                return direction.magnitude > 1f ? direction.normalized : direction;
            }

            public void OnGroundContactRegained()
            {
                Vector3 collisionVelocity = useLocalMomentum ? tr.localToWorldMatrix * momentum : momentum;
                OnLand.Invoke(collisionVelocity);
            }

            public void OnFallStart()
            {
                var currentUpMomemtum = VectorMath.ExtractDotVector(momentum, tr.up);
                momentum = VectorMath.RemoveDotVector(momentum, tr.up);
                momentum -= tr.up * currentUpMomemtum.magnitude;
            }

            public void OnGroundContactLost()
            {
                momentum = useLocalMomentum ? tr.localToWorldMatrix * momentum : momentum;
                Vector3 velocity = GetMovemenVelocity();
                if (velocity.sqrMagnitude >= 0f && momentum.sqrMagnitude >= 0f)
                {
                    Vector3 projectedMomentum = Vector3.ProjectOnPlane(momentum,velocity.normalized);
                    float dot = VectorMath.GetDotProduct(projectedMomentum.normalized,velocity.normalized);

                    if (projectedMomentum.sqrMagnitude >= velocity.sqrMagnitude && dot > 0f) velocity = Vector3.zero;
                    else if(dot > 0f) velocity -= projectedMomentum;
                }

                momentum += velocity;
                momentum = useLocalMomentum ? tr.worldToLocalMatrix * momentum : momentum;
            }

            public Vector3 GetMovemenVelocity() => savedVelocity;

            public void OnJumpStart()
            {
                momentum = useLocalMomentum ? tr.localToWorldMatrix * momentum : momentum;
                momentum+= tr.up * JumpSpeed;
                print(1);
                jumpTimer.Start();
                jumpInputIsLocked = true;
                OnJump.Invoke(momentum);
                momentum = useLocalMomentum ? tr.worldToLocalMatrix * momentum : momentum;
            }
    }
}