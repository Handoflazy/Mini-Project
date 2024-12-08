using UnityEngine;

namespace AdvancePlayerController.State_Machine
{
    public class IdleState : BaseState
    {
        public IdleState(Protagonist player, Animator animator) : base(player, animator)
        {
        }

        public override void OnEnter()
        {
            player.OnGroundContactRegained();
        }

        public override void Update()
        {
            animator.SetBool(WalkHash,false);
            animator.SetBool(AttackHash,false);
            animator.SetBool(AirHash,false);
            player.ClearInput();
            player.StopMovement();
        }
    }
}