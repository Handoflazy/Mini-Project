using UnityEngine;
namespace Platformer.AdvancePlayerController
{   
    [RequireComponent(typeof(PlayerController))]
    public class AnimationController: MonoBehaviour
    {
        PlayerController controller;
        private Animator animator;
        readonly int speedHash = Animator.StringToHash("Speed");
        readonly int isJumpingHash = Animator.StringToHash("IsJumping");
        
        void Start() {
            controller = GetComponent<PlayerController>();
            animator = GetComponentInChildren<Animator>();
            
            controller.OnJump += HandleJump;
            controller.OnLand += HandleLand;
        }
        void Update() {
            animator.SetFloat(speedHash, controller.GetMovemenVelocity().magnitude);
        }
        private void HandleLand(Vector2 obj)
        {
           
        }

        private void HandleJump(Vector2 obj)
        {
           
        }
        
        
    }
}