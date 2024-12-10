using AdvancePlayerController;
using UnityEngine;

namespace State
{
    public class SlidingState : BaseState
    {
        public SlidingState(Protagonist player, Animator animator) : base(player, animator)
        {
        }

        public override void OnEnter()
        {
            player.OnGroundContactRegained();
            
            animator.SetBool(AirboneHash,false);
            animator.SetBool(WalkHash,false);
            
            player.StopMovement();
        }
    }
}