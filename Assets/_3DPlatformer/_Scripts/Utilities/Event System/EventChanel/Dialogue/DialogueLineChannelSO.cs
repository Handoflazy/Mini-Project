using System;
using Platformer;
using Platformer.Dialogue;
using UnityEngine;
using UnityEngine.Events;

namespace Utilities.Event_System.EventChannel
{
    [CreateAssetMenu(menuName = "Events/UI/Dialogue line Channel")]
    public class DialogueLineChannelSO : DescriptionBaseSO
    {
        public UnityAction<String, ActorSO> OnEventRaised;

        public void RaiseEvent(String line, ActorSO actor) => OnEventRaised?.Invoke(line,actor);
    }
}