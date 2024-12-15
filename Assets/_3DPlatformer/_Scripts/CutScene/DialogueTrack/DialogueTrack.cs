using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using Utilities.EventChannel;

namespace Platformer.Dialogue
{
    [TrackClipType(typeof(DialogClip))]
    public class DialogueTrack : PlayableTrack
    {
        [SerializeField,Required] public DialogueLineChannelSO PlayDialogueEvent;
        [SerializeField,Required] public VoidEventChannel PauseTimelineEvent;
        [SerializeField,Required] public VoidEventChannel LineEndedEvent;
        public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
        {
            foreach (TimelineClip clip in GetClips())
            {
                DialogClip dialogClip = clip.asset as DialogClip;
                    dialogClip.PlayDialogueEvent = PlayDialogueEvent;
                    dialogClip.PauseTimelineEvent = PauseTimelineEvent;
                    dialogClip.LineEndedEvent = LineEndedEvent;
            }
            return base.CreateTrackMixer(graph, go, inputCount);
        }
        
    }
}