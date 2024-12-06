using AdvancePlayerController;
using UnityEngine;

namespace AdvancePlayerController.State_Machine
{
    public class SlidingState : BaseState
    {
        public SlidingState(Protagonist player, Animator animator) : base(player, animator)
        {
        }

        public override void OnEnter()
        {
            player.OnGroundContactRegained();
        }
    }
}