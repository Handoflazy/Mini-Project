using System;
using Utilities.ImprovedTimers;
using AdvancePlayerController.State_Machine;
using Platformer._Scripts.ScriptableObject;
using Platformer.Advanced;
using Character;
using Platformer.Systems.AudioSystem;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityUtils;
using Utilities.Event_System.EventBus;
using Utilities.Event_System.EventChannel;
using IPredicate = AdvancePlayerController.State_Machine.IPredicate;

namespace AdvancePlayerController
{
    [RequireComponent(typeof(Damageable))]
    public class Protagonist : MonoBehaviour
    {
            [Header("Elements")]
            [SerializeField,Required] Platformer.InputReader input;
            [SerializeField,Required] PlayerData data;
            [SerializeField,Required] private CeilingDetector ceilingDetector;
            [SerializeField,Required] Transform cameraTransform;
            [SerializeField,Required] private Animator animator;
            [SerializeField,Required] private Attacker attacker;
            [SerializeField,Required] private Damageable damageable;
            
            
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
            private CountdownTimer surprisedTimer;

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
            [SerializeField] private float surprisedAnimationTime =1f;
            [ReadOnly] [SerializeField] private string currentState;
           
            private Vector3 momentum, savedVelocity, savedMovementVelocity;
            private bool isJumpButtonHeld;
            [SerializeField]
            // ex: for animation effect
            public UnityEvent OnJump;
            public UnityEvent OnLand;
            
            public event Action OnAttack = delegate { };
            public event Action OnRun = delegate { };

            private bool attackInput;

            [SerializeField] private AudioCueSO landSound;
            [FormerlySerializedAs("vfxChannelSo")] [SerializeField] private AudioCueEventChannelSO vfxEventChannelSo;
            [SerializeField] private AudioConfigurationSO landSettings;

            private void Awake()
            {
                tr = transform;
                mover = GetComponent<PlayerMover>();
                SetUpTimers();
                SetupStateMachine();
            }
            private void Start() => input.EnablePlayerActions();


            void FixedUpdate()
            {
                stateMachine.FixedUpdate();
                mover.CheckForGround();
                HandleMomentum();
                var velocity = stateMachine.CurrentState is LocomotionState or WalkAttackState? CalculateMovementVelocity():Vector3.zero;
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
                input.JumpEvent += OnJumpInitiated;
                input.JumpCanceledEvent += OnJumpCanceled;
                input.StartedRunning += OnStartedSprinting;
                input.StoppedRunning += OnStoppedSprinting;
                input.AttackEvent += OnStartedAttack;
                input.AttackCanceledEvent += OnStopAttack;
                //

            }

           

            private void OnDisable()
            {
                input.JumpEvent -= OnJumpInitiated;
                input.JumpCanceledEvent -= OnJumpCanceled;
                input.StartedRunning -= OnStartedSprinting;
                input.StoppedRunning -= OnStoppedSprinting;
                input.AttackEvent -= OnStartedAttack;
                //
                
            }
            private void SetUpTimers()
            {
                jumpBuffer = new CountdownTimer(data.JumpInputBufferTime);
                sprintTimer = new CountdownTimer(runTime);
                runCooldownTimer = new CountdownTimer(runCooldownTime);
                surprisedTimer = new CountdownTimer(surprisedAnimationTime);
                
                attackCooldownTimer = new CountdownTimer(attackCooldown);
                attackTimer = new CountdownTimer(attackTime);
                attackTimer.OnTimerStop += () => attackCooldownTimer.Start();
                
                sprintTimer.OnTimerStop += () => runCooldownTimer.Start();
                sprintTimer.OnTimerStop += OnStoppedSprinting;
            }
           

            private void SetupStateMachine()
            {
                stateMachine = new StateMachine();
                
                var locomotionState = new LocomotionState(this, animator);
                var jumpState = new JumpState(this, animator);
                var slidingState = new SlidingState(this, animator);
                var risingState = new RisingState(this, animator);
                var fallingState = new FallingState(this, animator);
                var idleAttackState = new IdleAttackState(this, animator);
                var walkAttackState = new WalkAttackState(this, animator);
                var dyingState = new DyingState(this, animator);
                var gettingHitState = new GetttingHit(this, animator, damageable,surprisedTimer);

                At(locomotionState,walkAttackState,new FuncPredicate(()=>attackInput&&GetInputVelocity()!=Vector3.zero));
                At(locomotionState,idleAttackState,new FuncPredicate(()=>attackInput&&GetInputVelocity()==Vector3.zero));
                
                At(locomotionState,slidingState,new FuncPredicate(IsGroundTooSteep));
                At(locomotionState,jumpState, new FuncPredicate(() => jumpBuffer.IsRunning));
                At(locomotionState,risingState, new FuncPredicate(IsRising));
                At(locomotionState,fallingState,new FuncPredicate(IsFalling));
                At(locomotionState,fallingState,new FuncPredicate(()=>!mover.IsGrounded()));
                At(locomotionState,gettingHitState,new FuncPredicate(()=>damageable.GetHit));

                At(slidingState,locomotionState, new FuncPredicate(()=>!IsGroundTooSteep()));
                
                At(jumpState, risingState, new FuncPredicate(IsRising));
                At(jumpState,locomotionState,new FuncPredicate(()=>mover.IsGrounded()));
                
                At(risingState,fallingState, new FuncPredicate(IsFalling));
                At(risingState,fallingState, new FuncPredicate(()=>!isJumpButtonHeld));
                At(risingState,fallingState, new FuncPredicate(()=>ceilingDetector.HitCeiling()));
                
                At(fallingState,locomotionState, new FuncPredicate(()=>mover.IsGrounded()));
                
                At(idleAttackState,locomotionState, new FuncPredicate(()=>!attackInput));
                At(idleAttackState,gettingHitState, new FuncPredicate(()=>damageable.GetHit));
                
                At(walkAttackState,locomotionState, new FuncPredicate(()=>!attackInput));
                At(walkAttackState,gettingHitState, new FuncPredicate(()=>damageable.GetHit));
                At(walkAttackState,idleAttackState, new FuncPredicate(()=>GetInputVelocity()==Vector3.zero));
                
                At(gettingHitState,dyingState,new FuncPredicate(()=>damageable.IsDead));
                At(gettingHitState,locomotionState, new FuncPredicate(()=>!surprisedTimer.IsFinished));
                
                Any(gettingHitState,new FuncPredicate(()=>damageable.GetHit));
                stateMachine.SetState(fallingState);


            }

        

            void At(IState from, IState to, IPredicate codition) => stateMachine.AddTransition(from, to, codition);
            void Any(IState to, IPredicate codition) =>stateMachine.AddAnyTransition(to, codition);
            Vector2 GetMomentum() => useLocalMomentum?tr.localToWorldMatrix * momentum: momentum;

            bool IsRising()=>VectorMath.GetDotProduct(GetMomentum(), tr.up) > 0f;
            bool IsFalling()=>VectorMath.GetDotProduct(GetMomentum(), tr.up) <0f;
            public bool IsRunning() => sprintTimer.IsRunning;
            bool IsGroundTooSteep()=>Vector3.Angle(mover.GetGroundNormal(), tr.up) > data.SlopeLimit;

            private void Update()
            {
                stateMachine.Update();
            }

            private void HandleMomentum()
            {
                if(useLocalMomentum) momentum = tr.localToWorldMatrix * momentum; //_tf.localToWorldMatrix.MultipleVector(momentum)
                Vector3 verticalMomentum = VectorMath.ExtractDotVector(momentum, tr.up);
                Vector3 horizontalMomentum = momentum - verticalMomentum;
                verticalMomentum = HandleGravity(verticalMomentum);

                if (stateMachine.CurrentState is LocomotionState or WalkAttackState &&
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
                switch (stateMachine.CurrentState)
                {
                    case JumpState or RisingState:
                        verticalMomentum -= transform.up*(data.Gravity*data.GravityScale*Time.deltaTime);
                        break;
                    case FallingState:
                        verticalMomentum -= transform.up*(data.Gravity*data.FallGravityMult*Time.deltaTime);
                        break;
                }

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

            private bool IsGroundedStates() => stateMachine.CurrentState is LocomotionState or SlidingState or IdleAttackState or WalkAttackState;

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
                Vector3 direction = cameraTransform==null ? tr.right* input.Direction.x+tr.forward*input.Direction.y
                : Vector3.ProjectOnPlane(cameraTransform.right, Vector3.up).normalized*input.Direction.x + 
                Vector3.ProjectOnPlane(cameraTransform.forward, Vector3.up).normalized*input.Direction.y ;
                return direction.magnitude > 1f ? direction.normalized : direction;
            }

            public void OnGroundContactRegained()
            {
                Vector3 collisionVelocity = useLocalMomentum ? tr.localToWorldMatrix * momentum : momentum;
                OnLand.Invoke();
                /*if(collisionVelocity.magnitude>0.1f)
                    vfxChannelSo.Invoke(landSound, landSettings,transform.position);*/
                momentum = Vector3.zero;
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

            public void JumpStart()
            {
                momentum = useLocalMomentum ? tr.localToWorldMatrix * momentum : momentum;
                momentum += tr.up * data.JumpForce;
                OnJump.Invoke();
                momentum = useLocalMomentum ? tr.worldToLocalMatrix * momentum : momentum;
            }
            

            public void Die()
            {
                StopMovement();
                input.DisablePlayerActions();
            }
        
            public void StopMovement()
            {
                mover.SetVelocity(Vector3.zero);
            }
            //Clears the Protagonist's cached input of actions like Jump
            public void ClearInput()
            {
                if(jumpBuffer.IsRunning)
                    jumpBuffer.Stop();
            }

            #region ---- EVENT LISTENERS ----
            void OnJumpInitiated()
            {
                if (!jumpBuffer.IsRunning)
                {
                    jumpBuffer.Start();
                }
                isJumpButtonHeld = true;
            }
            void OnJumpCanceled()
            {
                isJumpButtonHeld = false;
            }
            
            private void OnStartedSprinting()
            {
                if (sprintTimer.IsRunning || runCooldownTimer.IsRunning) return;
                OnRun.Invoke();
                sprintTimer.Start();

            }
            private void OnStoppedSprinting()
            {
                OnRun.Invoke();
                sprintTimer.Stop();
            }
            private void OnStartedAttack()
            {
                attackInput = true;
            }

            private void OnStopAttack()
            {
                attackInput = false;
            }

            #endregion
           
            
            
            
    }
}