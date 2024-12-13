using Platformer.CutScenes;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Platformer.Dialogue
{
    [TrackClipType(typeof(DialogControlClip))]
    [TrackBindingType(typeof(CutsceneManager))]
    public class DialogueControlTrack : PlayableTrack
    {
        public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
        {
            var scriptPlayable = ScriptPlayable<DialogueControlMixerBehaviour>.Create(graph, inputCount); 
            DialogueControlMixerBehaviour behaviour = scriptPlayable.GetBehaviour();
            foreach (TimelineClip clip in GetClips())
            {
                // Needed?
            }

            return scriptPlayable;
        }
        
    }
}