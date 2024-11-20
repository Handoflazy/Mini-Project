using AdvancePlayerController;
using UnityEngine;

namespace AdvancePlayerController.State_Machine
{
    public class AttackState : BaseState
    {
        public AttackState(PlayerController player, Animator animator) : base(player, animator)
        {
        }

        public override void OnEnter()
        {
            animator.Play(AttackHash);
        }
    }
}