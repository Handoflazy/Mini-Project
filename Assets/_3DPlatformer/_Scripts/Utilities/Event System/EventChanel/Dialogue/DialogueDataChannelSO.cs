using Platformer;
using Platformer.Dialogue;
using UnityEngine;
using UnityEngine.Events;

namespace Utilities.Event_System.EventChannel
{
    /// <summary>
    /// This class is used for talk interaction events.
    /// Example: start talking to an actor passed as paramater
    /// </summary>
    [CreateAssetMenu(menuName = "Events/Dialogue Data Channel")]
    public class DialogueDataChannelSO : DescriptionBaseSO
    {
        public UnityAction<DialogueDataSO> OnEventRaised;

        public void RaiseEvent(DialogueDataSO dialogue)
        {
            OnEventRaised?.Invoke(dialogue);
        }
    }
}