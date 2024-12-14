using Platformer.CutScenes;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Platformer.Dialogue
{
    [TrackClipType(typeof(DialogClip))]
    [TrackBindingType(typeof(CutsceneManager))]
    public class DialogueTrack : PlayableTrack
    {
        public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
        {
            CutsceneManager cutsceneManagerRef = go.GetComponent<PlayableDirector>().GetGenericBinding(this) as CutsceneManager;
            foreach (TimelineClip clip in GetClips())
            {
                DialogClip dialogueControlClip = clip.asset as DialogClip;
                if (dialogueControlClip != null) dialogueControlClip.cutsceneManager = cutsceneManagerRef;
            }

            return base.CreateTrackMixer(graph, go, inputCount);
        }
        
    }
}