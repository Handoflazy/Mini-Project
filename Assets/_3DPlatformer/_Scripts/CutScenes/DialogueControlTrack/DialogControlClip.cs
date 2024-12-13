using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Platformer.Dialogue
{
    public class DialogControlClip : PlayableAsset,  ITimelineClipAsset
    {
        [SerializeField] private DialogueControlBehaviour template = new DialogueControlBehaviour();
        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            ScriptPlayable<DialogueControlBehaviour> playable = ScriptPlayable<DialogueControlBehaviour>.Create(graph, template);
            return playable;
        }

        public ClipCaps clipCaps
        {
            get { return ClipCaps.None; }
        }

    }
}