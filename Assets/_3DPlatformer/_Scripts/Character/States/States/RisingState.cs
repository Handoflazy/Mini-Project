using AdvancePlayerController;
using UnityEngine;

namespace State
{
    public class RisingState: BaseState
    {
        public RisingState(Protagonist player, Animator animator) : base(player, animator)
        {
        }

        public override void OnEnter()
        {
            animator.SetBool(AirboneHash,true);
        }
        
    }
}