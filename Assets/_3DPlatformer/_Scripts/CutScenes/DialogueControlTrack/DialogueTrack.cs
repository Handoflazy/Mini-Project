using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using Utilities.Event_System.EventChannel;

namespace Platformer.Dialogue
{
    [TrackClipType(typeof(DialogClip))]
    public class DialogueTrack : PlayableTrack
    {
        [SerializeField] public DialogueLineChannelSO PlayDialogueEvent;
        [SerializeField] public VoidEventChannel PauseTimelineEvent;
        public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
        {
            foreach (TimelineClip clip in GetClips())
            {
                DialogClip dialogClip = clip.asset as DialogClip;
                    dialogClip.PlayDialogueEvent = PlayDialogueEvent;
                    dialogClip.PauseTimelineEvent = PauseTimelineEvent;
            }
            return base.CreateTrackMixer(graph, go, inputCount);
        }
        
    }
}