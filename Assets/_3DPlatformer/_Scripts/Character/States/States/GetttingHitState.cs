using Character;
using UnityEngine;
using Utilities.ImprovedTimers;

namespace AdvancePlayerController.State_Machine
{
    public class GetttingHitState : BaseState
    {
        private readonly Damageable playerDamageable;
        private CountdownTimer surprisedTimer;
        public GetttingHitState(Protagonist player, Animator animator, Damageable damageable, CountdownTimer surprisedTimer) : base(player, animator)
        {
            this.surprisedTimer = surprisedTimer;
            playerDamageable = damageable;
        }

        public override void OnEnter()
        { 
            //Sets a Parameter on the character's Animator controller
            animator.SetTrigger(HitHash);
            //Sets a Parameter on the character's Animator controller
            animator.SetBool(SurprisedHash,true);
            //Resets GetHit bool on the Damageable script
            playerDamageable.GetHit = false;
            surprisedTimer.Start();
            //TODO: ShakeCam && Flashing Effect

        }

        public override void OnExit()
        {
            animator.SetBool(SurprisedHash,false);
        }
    }
}