using Character.PlayerParticleSystem;
using Unity.VisualScripting;
using UnityEngine;
namespace Character
{   
    [RequireComponent(typeof(global::AdvancePlayerController.Protagonist))]
    public class AnimationController: MonoBehaviour
    {
        global::AdvancePlayerController.Protagonist controller;
        private Animator animator;
        private PlayerParticles particles;
        readonly int speedHash = Animator.StringToHash("Speed");    
        readonly int JumpHash = Animator.StringToHash("Jump");
        readonly int RunHash = Animator.StringToHash("IsRunning");
        private readonly int AttackHash = Animator.StringToHash("Attack");
        private readonly int DieHash = Animator.StringToHash("Die");
        private readonly int HitHash = Animator.StringToHash("Hit");
        private float smoothSpeed = 0;
        [SerializeField] private float smoothSpeedMultilier = 1f;
        [SerializeField] private float crossFade = 0.25f;
        
        
        void Start() {
            controller = GetComponent<global::AdvancePlayerController.Protagonist>();
            animator = GetComponentInChildren<Animator>();
            particles = GetComponentInChildren<PlayerParticles>();
            
            controller.OnJump += HandleJump;
            controller.OnLand += HandleLand;
            controller.OnRun += HandleRun;
            controller.OnAttack += HandleAttack;
        }

        private void HandleDeath()
        {
            animator.CrossFade(DieHash,crossFade);
        }

        private void OnDisable()
        {
            controller.OnJump -= HandleJump;
            controller.OnLand -= HandleLand;
            controller.OnRun -= HandleRun;
            controller.OnAttack -= HandleAttack;
        }
        void Update()
        {

            smoothSpeed = Mathf.SmoothStep(smoothSpeed, controller.GetInputVelocity().magnitude, Time.deltaTime*smoothSpeedMultilier);
            animator.SetFloat(speedHash, smoothSpeed);
            
            /*if (!controller.IsGrounded())
            {
                animator.SetFloat(JumpHash, 0.5f);
                if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Jump"))
                {
                    animator.Play(JumpHash);
                }
            }*/

            
            
        }

        private void HandleRun(bool isRunning)
        {
            animator.SetBool(RunHash, isRunning);
        }
        private void HandleLand(Vector2 obj)
        {
            animator.SetFloat(JumpHash, 1);
            animator.CrossFade("Locomotion",crossFade);
        }

        private void HandleJump(Vector2 obj)
        {
            animator.SetFloat(JumpHash, 0);
            animator.CrossFade(JumpHash,crossFade);
        }

        private void HandleAttack()
        {
            animator.CrossFade(AttackHash,crossFade);
            particles.PlaySlash();
        }

        private void HandleHit()
        {
            animator.CrossFade(HitHash,crossFade);
        }
        
        
    }
}