using System;
using Utilities.ImprovedTimers;
using AdvancePlayerController.State_Machine;
using Platformer._Scripts.ScriptableObject;
using Platformer.Advanced;
using Platformer.AdvancePlayerController;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;
using UnityUtils;
using Utilities.Event_System.EventBus;
using IPredicate = AdvancePlayerController.State_Machine.IPredicate;

namespace AdvancePlayerController
{
    [RequireComponent(typeof(Damageable))]
    public class Protagonist : MonoBehaviour
    {
        #region Fields
            [Header("Elements")]
            [SerializeField] Platformer.InputReader inputReader;
            [SerializeField] PlayerData data;
            [SerializeField] private CeilingDetector ceilingDetector;
            [SerializeField] Transform cameraTransform;
            [SerializeField] private Animator animator;
            [SerializeField] private Attacker attacker;
            public bool useLocalMomentum;

            private Transform tr;
            private PlayerMover mover;
            private StateMachine stateMachine;

            #region Timers

            private CountdownTimer jumpBuffer;
            private CountdownTimer sprintTimer;
            private CountdownTimer runCooldownTimer;
            private CountdownTimer attackCooldownTimer;
            private CountdownTimer attackTimer;

            #endregion
            

            [Header("Attack Settings")] [SerializeField]
            private float attackTime;
            [SerializeField] private float attackCooldown;
            
        
            private bool isRunPressing;
            [Header("Run Settings")]
            [SerializeField]
            private float RunMultiplier = 1.5f;

            [SerializeField] private float runCooldownTime = 2f;
            [SerializeField] private float runTime = 3f;
            [ReadOnly] [SerializeField] private string currentState;
           
            private Vector3 momentum, savedVelocity, savedMovementVelocity;
            private bool isJumpButtonHeld;
            private bool isDeath;
            
            // ex: for animation effect
            public event Action<Vector2> OnJump = delegate { };
            public event Action<Vector2> OnLand = delegate { };
            public event Action OnAttack = delegate { };
            public event Action<bool> OnRun = delegate { };


            private EventBinding<PlayerEvents> playerEventBinding;
            private EventBinding<TestEvents> testEventBinding;

            #endregion

            private void Awake()
            {
                tr = transform;
                mover = GetComponent<PlayerMover>();
                SetUpTimers();
                SetupStateMachine();
            }

            void HandleTestEvent()
            {
                print("Test event Received");
            }
            void HandlePlayerEvent(PlayerEvents playerEvent){
                print("Player event Reiceived ");
            }

            private void Start()
            {
                inputReader.Jump += HandleJumpKeyInput;
                inputReader.Dash +=HandleSprintKey;
                inputReader.Attack += HandleAttackInput;
                inputReader.EnablePlayerActions();
                
            }
            void FixedUpdate()
            {
                if (playerDamageable.IsDead)
                    return;
                stateMachine.FixedUpdate();
                mover.CheckForGround();
                HandleMomentum();
                Vector3 velocity = stateMachine.CurrentState is LocomotionState? CalculateMovementVelocity():Vector3.zero;
                velocity = sprintTimer.IsRunning? velocity*RunMultiplier: velocity;
                velocity += useLocalMomentum ? tr.localToWorldMatrix * momentum : momentum;
                mover.SetExtendSensorRange(IsGroundedStates());
                mover.SetVelocity(velocity);
                savedVelocity = velocity;
                savedMovementVelocity = CalculateMovementVelocity();
                
                ceilingDetector.Reset();
                
               
            }

            private void OnEnable()
            {
                testEventBinding = new EventBinding<TestEvents>(HandleTestEvent);
                EventBus<TestEvents>.Register(testEventBinding);
                playerEventBinding = new EventBinding<PlayerEvents>(HandlePlayerEvent);
                EventBus<PlayerEvents>.Register(playerEventBinding);
            }

            private void OnDisable()
            {
                inputReader.Jump -= HandleJumpKeyInput;
                inputReader.Dash -= HandleSprintKey;
                inputReader.Attack -= HandleAttackInput;
                
                EventBus<TestEvents>.Deregister(testEventBinding);
                EventBus<PlayerEvents>.Deregister(playerEventBinding);
            }
            private void SetUpTimers()
            {
                jumpBuffer = new CountdownTimer(data.JumpInputBufferTime);
                
                sprintTimer = new CountdownTimer(runTime);
                runCooldownTimer = new CountdownTimer(runCooldownTime);
                
                attackCooldownTimer = new CountdownTimer(attackCooldown);
                attackTimer = new CountdownTimer(attackTime);
                attackTimer.OnTimerStop += () => attackCooldownTimer.Start();
                
                sprintTimer.OnTimerStop += () => runCooldownTimer.Start();
                sprintTimer.OnTimerStop += StopSprint;
            }
            private void HandleAttackInput()
            {
                //Try to Attack
                if(!attackTimer.IsRunning&& !attackCooldownTimer.IsRunning && stateMachine.CurrentState is LocomotionState)
                    attackTimer.Start();
            }

            private void HandleSprintKey(bool isPerformSprint)
            {
                if (savedVelocity != Vector3.zero && isPerformSprint&& !sprintTimer.IsRunning&&!runCooldownTimer.IsRunning)
                {
                    OnRun.Invoke(true);
                    sprintTimer.Start();
                }
                else if (!isPerformSprint)
                {
                    StopSprint();
                }
                
            }

            private void StopSprint()
            {
                OnRun.Invoke(false);
                sprintTimer.Stop();
            }

            


            private void SetupStateMachine()
            {
                stateMachine = new StateMachine();
                
                var locomotionState = new LocomotionState(this, animator);
                var jumpState = new JumpState(this, animator);
                var slideState = new SlidingState(this, animator);
                var risingState = new RisingState(this, animator);
                var fallingState = new FallingState(this, animator);
                var attackState = new AttackState(this, animator);
                var dieState = new DeathState(this, animator);

                At(locomotionState, jumpState, new FuncPredicate(() => jumpBuffer.IsRunning));
                At(locomotionState, risingState, new FuncPredicate(IsRising));
                At(locomotionState,slideState,new FuncPredicate(IsGroundTooSteep));
                At(locomotionState,fallingState,new FuncPredicate(IsFalling));
                At(locomotionState,fallingState,new FuncPredicate(()=>!mover.IsGrounded()));
                At(locomotionState,attackState,new FuncPredicate(()=>attackTimer.IsRunning));

                At(slideState,locomotionState, new FuncPredicate(()=>!IsGroundTooSteep()));
                
                At(jumpState, risingState, new FuncPredicate(IsRising));
                
                At(risingState,fallingState, new FuncPredicate(IsFalling));
                At(risingState,fallingState, new FuncPredicate(()=>!isJumpButtonHeld));
                At(risingState,fallingState, new FuncPredicate(()=>ceilingDetector.HitCeiling()));
                
                At(fallingState,locomotionState, new FuncPredicate(()=>mover.IsGrounded()));
                
                At(attackState,locomotionState, new FuncPredicate(()=>attackTimer.IsFinished));
                
                Any(dieState,new FuncPredicate(()=>isDeath&&IsGroundedStates()));
                stateMachine.SetState(fallingState);


            }

        

            void At(IState from, IState to, IPredicate codition) => stateMachine.AddTransition(from, to, codition);
            void Any(IState to, IPredicate codition) =>stateMachine.AddAnyTransition(to, codition);
            public Vector2 GetMomentum() => useLocalMomentum?tr.localToWorldMatrix * momentum: momentum;

            private bool IsRising()=>VectorMath.GetDotProduct(GetMomentum(), tr.up) > 0f;
            private bool IsFalling()=>VectorMath.GetDotProduct(GetMomentum(), tr.up) <0f;
            public bool IsRunning() => sprintTimer.IsRunning;
            bool IsGroundTooSteep()=>Vector3.Angle(mover.GetGroundNormal(), tr.up) > data.SlopeLimit;






            private void Update()
            {
                stateMachine.Update();
            }
            
                

            void HandleJumpKeyInput(bool isButtonPressed)
            {
                if (!jumpBuffer.IsRunning && isButtonPressed)
                {
                    jumpBuffer.Start();
                }

                if (jumpBuffer.IsRunning && !isButtonPressed)
                {
                    jumpBuffer.Stop();
                }

                isJumpButtonHeld = isButtonPressed;
            }

            private void HandleMomentum()
            {
                if(useLocalMomentum) momentum = tr.localToWorldMatrix * momentum; //_tf.localToWorldMatrix.MultipleVector(momentum)
                Vector3 verticalMomentum = VectorMath.ExtractDotVector(momentum, tr.up);
                Vector3 horizontalMomentum = momentum - verticalMomentum;
                verticalMomentum = HandleGravity(verticalMomentum);

                if (stateMachine.CurrentState is LocomotionState &&
                    VectorMath.GetDotProduct(verticalMomentum, tr.up) < 0f)
                    verticalMomentum = Vector3.zero;
                if (!IsGroundedStates())
                {
                    AdjustHorinzontalMomentum(ref horizontalMomentum, CalculateMovementVelocity());
                }
                
                if (stateMachine.CurrentState is SlidingState) 
                {
                    HandleSliding(ref horizontalMomentum);
                }
                float friction = stateMachine.CurrentState is LocomotionState ? data.GroundFriction: data.AirFriction;
                
                horizontalMomentum = Vector3.MoveTowards(horizontalMomentum,Vector3.zero,friction*Time.deltaTime);
                momentum = horizontalMomentum + verticalMomentum;
                if (stateMachine.CurrentState is JumpState) {
                    HandleJumping();
                }
                if (stateMachine.CurrentState is SlidingState) {
                    momentum = Vector3.ProjectOnPlane(momentum, mover.GetGroundNormal());
                    if (VectorMath.GetDotProduct(momentum, tr.up) > 0f) {
                        momentum = VectorMath.RemoveDotVector(momentum, tr.up);
                    }
            
                    Vector3 slideDirection = Vector3.ProjectOnPlane(-tr.up, mover.GetGroundNormal()).normalized;
                    momentum += slideDirection * (data.SlideGravity * Time.deltaTime);
                }
                
   
                
                
                if(useLocalMomentum) momentum = tr.worldToLocalMatrix * momentum;
            }

            private Vector3 HandleGravity(Vector3 verticalMomentum)
            {
                if (stateMachine.CurrentState is JumpState or RisingState)
                {
                    verticalMomentum -= transform.up*(data.Gravity*data.GravityScale*Time.deltaTime);
                }
                else if(stateMachine.CurrentState is FallingState)
                    verticalMomentum -= transform.up*(data.Gravity*data.FallGravityMult*Time.deltaTime);

                float maxFallSpeed = data.MaxFallSpeed;
                if (verticalMomentum.magnitude > maxFallSpeed)
                {
                    verticalMomentum = verticalMomentum.normalized * maxFallSpeed;
                }
                return verticalMomentum;
            }


            private void HandleSliding(ref Vector3 horizontalMomentum)
            {
                Vector3 pointDownVector = Vector3.ProjectOnPlane(mover.GetGroundNormal(), tr.up).normalized;
                Vector3 movementVelocity = CalculateMovementVelocity();
                movementVelocity = VectorMath.RemoveDotVector(movementVelocity, pointDownVector);
                horizontalMomentum += movementVelocity*Time.fixedDeltaTime; 
            }

            public bool IsGroundedStates() => stateMachine.CurrentState is LocomotionState or SlidingState or AttackState;

            void AdjustHorinzontalMomentum(ref Vector3 horizontalMomentum, Vector3 movementVelocity)
            {
                
                if (horizontalMomentum.magnitude > data.RunMaxSpeed)
                {
                    if (VectorMath.GetDotProduct(movementVelocity, horizontalMomentum.normalized) > 0f)
                    {
                        movementVelocity = VectorMath.RemoveDotVector(movementVelocity, horizontalMomentum.normalized);
                    }
                    horizontalMomentum += movementVelocity * (data.AirControlRate * Time.deltaTime*0.25f);
                }
                else
                {   
                    horizontalMomentum += movementVelocity * (data.AirControlRate * Time.deltaTime);
                    horizontalMomentum = Vector3.ClampMagnitude(horizontalMomentum, data.RunMaxSpeed);
                   
                }
               
               
            }
            private Vector3 CalculateMovementVelocity() => CalculateMovementDirection() * data.RunMaxSpeed;
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
                momentum = Vector3.zero;
            }

            public void OnFallStart()
            {
                var currentUpMomemtum = VectorMath.ExtractDotVector(momentum, tr.up);
                momentum = VectorMath.RemoveDotVector(momentum, tr.up);
                momentum -= tr.up * currentUpMomemtum.magnitude;
            }

            public void OnGroundContactLost() // dieu chinh huong nhay
            {
                momentum = useLocalMomentum ? tr.localToWorldMatrix * momentum : momentum;
                Vector3 velocity = GetMovementVelocity();
                if (velocity.sqrMagnitude >= 0f && momentum.sqrMagnitude > 0f)
                {
                    Vector3 projectedMomentum = Vector3.Project(momentum,velocity.normalized);
                    float dot = VectorMath.GetDotProduct(projectedMomentum.normalized,velocity.normalized);
                    if (projectedMomentum.sqrMagnitude >= velocity.sqrMagnitude && dot > 0f) velocity = Vector3.zero;
                    else if(dot > 0f) velocity -= projectedMomentum;
                    
                }

                momentum += velocity;
                momentum = useLocalMomentum ? tr.worldToLocalMatrix * momentum : momentum;
            }

            public Vector3 GetInputVelocity() => savedMovementVelocity;
            public Vector3 GetMovementVelocity() => savedVelocity;

            public void OnJumpStart()
            {
                momentum = useLocalMomentum ? tr.localToWorldMatrix * momentum : momentum;
                //jumpTimer.Start();
                momentum += tr.up * data.JumpForce;
                OnJump.Invoke(momentum);
                
                momentum = useLocalMomentum ? tr.worldToLocalMatrix * momentum : momentum;
            }
            private void HandleJumping()
            {
                momentum = VectorMath.RemoveDotVector(momentum, tr.up);
                
                momentum += tr.up * data.JumpForce;
            }
            

            public void Die()
            {
                mover.SetVelocity(Vector3.zero);
                isDeath = true;
            }
    }
}