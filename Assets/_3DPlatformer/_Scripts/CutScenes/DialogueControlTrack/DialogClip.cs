using Platformer.CutScenes;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Platformer.Dialogue
{
    public class DialogClip : PlayableAsset,  ITimelineClipAsset
    {
        [SerializeField] private DialogueBehaviour template = new DialogueBehaviour();
        [HideInInspector] public CutsceneManager cutsceneManager;
        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            template.cutsceneManager = cutsceneManager;
            ScriptPlayable<DialogueBehaviour> playable = ScriptPlayable<DialogueBehaviour>.Create(graph, template);
            return playable;
        }

        public ClipCaps clipCaps
        {
            get { return ClipCaps.None; }
        }

    }
}