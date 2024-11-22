using AdvancePlayerController;
using UnityEngine;

namespace AdvancePlayerController.State_Machine
{
    public class RisingState: BaseState
    {
        public RisingState(Protagonist player, Animator animator) : base(player, animator)
        {
        }

        public override void OnEnter()
        {
            animator.SetBool(AirHash,true);
        }
        
    }
}