using AdvancePlayerController;
using Platformer._3DPlatformer._Scripts.Character;
using UnityEngine;
using Utilities.ImprovedTimers;

namespace AdvancePlayerController.State_Machine
{
    public class FallingState : BaseState
    {
        public FallingState(Protagonist player, Animator animator,PlayerEffectController dustController) : base(player, animator)
        {
        }

        public override void OnEnter()
        {
            animator.SetBool(AirHash,true);
        }

        public override void OnExit()
        {
            animator.SetBool(AirHash,false);
        }
    }
}