using Utilities.Event_System.EventBus;

namespace Platformer.AbilitySystem
{
    public interface ICommand
    {
        void Execute();
    }

    public class AbilityCommand : ICommand
    {
        private readonly AbilityData data;
        public float duration => data.Duration;

        public AbilityCommand(AbilityData data)
        {
            this.data = data;
        }
        public void Execute()
        {
            EventBus<PlayerAnimationEvent>.Raise(new PlayerAnimationEvent()
            {
                animationHash = data.AnimationHash
            });
        }
    }

    public struct PlayerAnimationEvent : IEvent
    {
        public int animationHash;
    }
}