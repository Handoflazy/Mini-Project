using System;
using Utilities.ImprovedTimers;
using State;
using Platformer._Scripts.ScriptableObject;
using Platformer.Advanced;
using Character;
using Platformer._3DPlatformer._Scripts.Character;
using Platformer.GamePlay;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityUtils;
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
            [SerializeField,Required] private PlayerEffectController playerEffectController;
            [SerializeField,Required] private ProtagonistAudio protagonistAudio;
            
            [SerializeField] private float MaxFallDistance = 8;
            [SerializeField] private float combatTime = 8;
            public bool useLocalMomentum;

            private Transform tr;
            private PlayerMover mover;
            private StateMachine stateMachine;

            #region Timers
            private CountdownTimer jumpBuffer;
            private CountdownTimer sprintTimer;
            private CountdownTimer runCooldownTimer;
            private CountdownTimer surprisedTimer;
        

            #endregion
            

        
            
        
            [Header("Run Settings")]
            [SerializeField]
            private float RunMultiplier = 1.5f;

            [SerializeField] private float runCooldownTime = 2f;
            [SerializeField] private float runTime = 3f;
            [SerializeField] private float surprisedAnimationTime =1f;
            [SerializeField] private float slideToJumpThreshold;
            [ReadOnly] [SerializeField] private string currentState;
           
            private Vector3 momentum, savedVelocity, savedMovementVelocity;
            [NonSerialized] private bool isRunPressing;
            [NonSerialized] private bool isJumpButtonHeld;
            [SerializeField] private bool attackInput;
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
                var velocity = stateMachine.CurrentState is WalkAttackState or WalkState? CalculateMovementVelocity():Vector3.zero;
                velocity = sprintTimer.IsRunning? velocity*RunMultiplier: velocity;
                velocity += useLocalMomentum ? tr.localToWorldMatrix * momentum : momentum;
                mover.SetExtendSensorRange(IsGroundedStates());
                mover.SetVelocity(velocity);
                savedVelocity = velocity;
                savedMovementVelocity = CalculateMovementVelocity();
                ceilingDetector.Reset();
                
               
            }

            public void UpdateCombatMode(bool isCombatMode)
            {
                animator.SetBool("IsCombat", isCombatMode);
            }

            private void OnEnable()
            {
                input.JumpEvent += OnJumpInitiated;
                input.JumpCanceledEvent += OnJumpCanceled;
                input.StartedRunning += OnStartedSprinting;
                input.StoppedRunning += OnStoppedSprinting;
                input.AttackEvent += OnStartedAttack;
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
                sprintTimer.OnTimerStop += () => runCooldownTimer.Start();
                sprintTimer.OnTimerStop += OnStoppedSprinting;
            }
           

            private void SetupStateMachine()
            {
                stateMachine = new StateMachine();
                stateMachine.OnStateChange += (t) => currentState = t.ToString();
                
                var idleState = new IdleState(this, animator);
                var walkState = new WalkState(this, animator, playerEffectController);
                var jumpState = new JumpState(this, animator,playerEffectController);
                var slidingState = new SlidingState(this, animator);
                var risingState = new RisingState(this, animator);
                var fallingState = new FallingState(this, animator, playerEffectController);
                var idleAttackState = new IdleAttackState(this, animator);
                var walkAttackState = new WalkAttackState(this, animator);
                var dyingState = new DyingState(this, animator);
                var gettingHitState = new GetttingHitState(this, animator, damageable,surprisedTimer);
                var jumpAttacking = new JumpAttacking(this, animator,playerEffectController);
                var fallingAttacking = new FallingAttackingState(this, animator);
                //TODO : DEFEND STATE
                #region Idle
                At(idleState,idleAttackState,new FuncPredicate(()=>attackInput));
                At(idleState,slidingState,new FuncPredicate(IsGroundTooSteep));
                At(idleState,jumpState, new FuncPredicate(() => jumpBuffer.IsRunning));
                At(idleState,jumpAttacking, new FuncPredicate(()=>jumpBuffer.IsRunning&&attackInput));
                At(idleState,fallingState,new FuncPredicate(IsFalling));
                At(idleState,fallingAttacking, new FuncPredicate(()=>!mover.IsGrounded()&&attackInput));
                //TODO: PICKUP STATE
                At(idleState,walkState,new FuncPredicate(()=>GetInputVelocity()!=Vector3.zero));
                At(idleState,walkAttackState, new FuncPredicate(()=>attackInput&&IsMoving()));
                At(idleState,risingState, new FuncPredicate(IsRising));
                At(idleState,fallingState,new FuncPredicate(()=>!mover.IsGrounded()));
                
                At(idleState,gettingHitState,new FuncPredicate(()=>damageable.GetHit));
                #endregion

                #region Walk

                At(walkState,slidingState,new FuncPredicate(IsGroundTooSteep));
                At(walkState,jumpState, new FuncPredicate(() => jumpBuffer.IsRunning));
                At(walkState,risingState, new FuncPredicate(IsRising));
                At(walkState,jumpAttacking, new FuncPredicate(()=>jumpBuffer.IsRunning&&attackInput));
                At(walkState,fallingState,new FuncPredicate(()=>!mover.IsGrounded()));
                //TODO: PICKUP STATE
                At(walkState,idleState,new FuncPredicate(()=>!IsMoving()));
                At(walkState,walkAttackState,new FuncPredicate(()=>attackInput&&IsMoving()));
                At(walkState,idleAttackState,new FuncPredicate(()=>attackInput&&!IsMoving()));
                
                At(walkState,gettingHitState,new FuncPredicate(()=>damageable.GetHit));
                #endregion

                #region Jump Ascending

                At(jumpState, risingState, new FuncPredicate(IsRising));
                At(jumpState,idleState,new FuncPredicate(mover.IsGrounded));
                
                At(risingState,fallingState, new FuncPredicate(IsFalling));
                At(risingState,fallingState, new FuncPredicate(()=>!isJumpButtonHeld));
                At(risingState,fallingState, new FuncPredicate(()=>ceilingDetector.HitCeiling()));

                At(risingState,fallingState, new FuncPredicate(()=>IsFalling()&&attackInput));
                At(risingState,fallingState, new FuncPredicate(()=>!isJumpButtonHeld&&attackInput));
                At(risingState,fallingState, new FuncPredicate(()=>ceilingDetector.HitCeiling()&&attackInput));
                
                At(risingState,jumpAttacking, new FuncPredicate(()=>attackInput));
                #endregion

                #region Falling
                At(fallingState,walkState, new FuncPredicate(()=>mover.IsGrounded()&&IsMoving()));
                At(fallingState,idleState, new FuncPredicate(()=>mover.IsGrounded()&&!IsMoving()));
                At(fallingState,fallingAttacking, new FuncPredicate(()=>attackInput));
                #endregion

                #region Jump attacking
                At(jumpAttacking,fallingState, new FuncPredicate(()=>!attackInput&&IsFalling()));
                At(jumpAttacking,fallingState, new FuncPredicate(()=>!attackInput&&!isJumpButtonHeld));
                At(jumpAttacking,fallingState, new FuncPredicate(()=>!attackInput&&ceilingDetector.HitCeiling()));
                
                At(jumpAttacking,fallingAttacking,new FuncPredicate(()=>attackInput&& !isJumpButtonHeld));
                At(jumpAttacking,fallingAttacking,new FuncPredicate(()=>attackInput&& IsFalling()));
                At(jumpAttacking,fallingAttacking,new FuncPredicate(()=>attackInput&& ceilingDetector.HitCeiling()));

                #endregion

                #region Falling attacking
                At(fallingAttacking,walkState, new FuncPredicate(()=>!attackInput&&mover.IsGrounded()&&IsMoving()));
                At(fallingAttacking,idleState, new FuncPredicate(()=>!attackInput&&mover.IsGrounded()&&!IsMoving()));
                
                At(fallingAttacking,fallingState, new FuncPredicate(()=>!attackInput&&!mover.IsGrounded()));
                #endregion
                
                #region Sliding
                At(slidingState,idleState, new FuncPredicate(()=>!IsGroundTooSteep()));
                At(slidingState,jumpState, new FuncPredicate(()=>jumpBuffer.IsRunning&&IsActuallyMoving(slideToJumpThreshold)));
                #endregion

                #region Idle Attacking
                At(idleAttackState,idleState, new FuncPredicate(()=>!attackInput&&!IsMoving()));
                At(idleAttackState,walkAttackState, new FuncPredicate(()=>!attackInput&&IsMoving()));
                At(idleAttackState,gettingHitState,new FuncPredicate(()=>damageable.GetHit));
                #endregion

                #region Walk Attacking
                At(walkAttackState,idleState, new FuncPredicate(()=>!attackInput&&!IsMoving()));
                At(walkAttackState,walkState, new FuncPredicate(()=>!attackInput&&IsMoving()));
                At(walkAttackState,gettingHitState,new FuncPredicate(()=>damageable.GetHit));
                #endregion
                
                At(gettingHitState,dyingState,new FuncPredicate(()=>damageable.IsDead));
                At(gettingHitState,idleState, new FuncPredicate(()=>!surprisedTimer.IsFinished));
                stateMachine.SetState(fallingState);
            }

            private bool IsMoving()
            {
                return GetInputVelocity()!=Vector3.zero;
            }

            private bool IsActuallyMoving(float threshold)
            {
                return momentum.sqrMagnitude < threshold * threshold;
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

                if (stateMachine.CurrentState is IdleState or IdleAttackState or WalkState or WalkAttackState &&
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
                var friction = HandleFriction();
                horizontalMomentum = Vector3.MoveTowards(horizontalMomentum,Vector3.zero,friction*Time.deltaTime);
                momentum = horizontalMomentum + verticalMomentum;
                if (stateMachine.CurrentState is SlidingState)
                {
                    HandleSliding();
                }
                
   
                
                
                if(useLocalMomentum) momentum = tr.worldToLocalMatrix * momentum;
            }

            private float HandleFriction()
            {
                float friction = IsGroundedStates() ? data.GroundFriction: data.AirFriction;
                if (stateMachine.CurrentState is SlidingState)
                    friction = data.AirFriction;
                return friction;
            }

            private void HandleSliding()
            {
                momentum = Vector3.ProjectOnPlane(momentum, mover.GetGroundNormal());
                if (VectorMath.GetDotProduct(momentum, tr.up) > 0f) {
                    momentum = VectorMath.RemoveDotVector(momentum, tr.up);
                }
            
                Vector3 slideDirection = Vector3.ProjectOnPlane(-tr.up, mover.GetGroundNormal()).normalized;
                momentum += slideDirection * (data.SlideGravity * Time.deltaTime);
            }

            private Vector3 HandleGravity(Vector3 verticalMomentum)
            {
                switch (stateMachine.CurrentState)
                {
                    case JumpState or RisingState or JumpAttacking:
                        verticalMomentum -= transform.up*(data.Gravity*data.GravityScale*Time.deltaTime);
                        break;
                    case FallingState or FallingAttackingState:
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

            private bool IsGroundedStates() => stateMachine.CurrentState is IdleState or SlidingState or IdleAttackState or WalkAttackState or WalkState;

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
                float maxSafeFallVelocity = Mathf.Sqrt(2*data.Gravity * data.FallGravityMult * MaxFallDistance);
                float fallIntensity = Mathf.InverseLerp(0, maxSafeFallVelocity, collisionVelocity.magnitude);
                playerEffectController.PlayLandParticles(fallIntensity);
                if(collisionVelocity.magnitude>maxSafeFallVelocity)
                    damageable.Kill(); // death by high fall
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
            public void ClearInputCache()
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
            void OnJumpCanceled() => isJumpButtonHeld = false;

            private void OnStartedSprinting()
            {
                if (sprintTimer.IsRunning || runCooldownTimer.IsRunning) return;
                sprintTimer.Start();

            }
            private void OnStoppedSprinting() => sprintTimer.Stop();
            private void OnStartedAttack()
            {
                attackInput = true;
            }

            public void ConsumeAttackInput() => attackInput = false;

            #endregion




    }
}