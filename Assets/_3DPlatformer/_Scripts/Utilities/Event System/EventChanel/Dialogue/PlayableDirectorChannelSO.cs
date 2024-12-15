using Platformer;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;

namespace Utilities.EventChannel
{
    [CreateAssetMenu(menuName = "Events/Playable Director Channel")]
    public class PlayableDirectorChannelSO : DescriptionBaseSO
    {
        public UnityAction<PlayableDirector> OnEventRaised;

        public void RaiseEvent(PlayableDirector director)
        {
            OnEventRaised?.Invoke(director);
        }
    }
}