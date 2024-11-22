using UnityEngine;

namespace AdvancePlayerController.State_Machine
{
    public class DeathState : BaseState
    {
        public DeathState(Protagonist player, Animator animator) : base(player, animator)
        {
        }

        public override void OnEnter()
        {
            animator.Play(DieHash);
        }
    }
}