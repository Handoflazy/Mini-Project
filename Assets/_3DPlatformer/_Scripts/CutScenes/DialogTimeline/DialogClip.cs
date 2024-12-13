using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Platformer.CutScenes.DialogClip
{
    public class DialogClip : PlayableAsset,  ITimelineClipAsset
    {
        public DialogBehaviour dialog = new DialogBehaviour();
        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            var playerAble = ScriptPlayable<DialogBehaviour>.Create(graph, dialog);
            return playerAble;
        }

        public ClipCaps clipCaps
        {
            get { return ClipCaps.None; }
        }

    }
}