using Platformer.CutScenes;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using Utilities.Event_System.EventChannel;

namespace Platformer.Dialogue
{
    public class DialogClip : PlayableAsset,  ITimelineClipAsset
    {
        [SerializeField] private DialogueBehaviour template = new DialogueBehaviour();

        [HideInInspector] public DialogueLineChannelSO PlayDialogueEvent;
        [HideInInspector] public VoidEventChannel PauseTimelineEvent;
        [HideInInspector] public VoidEventChannel LineEndedEvent;
        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            
            ScriptPlayable<DialogueBehaviour> playable = ScriptPlayable<DialogueBehaviour>.Create(graph, template);
            template.PauseTimeLineEvent = PauseTimelineEvent;
            template.PlayDialogueEvent = PlayDialogueEvent;
            template.LineEndedEvent = LineEndedEvent;
            return playable;
        }

        public ClipCaps clipCaps
        {
            get { return ClipCaps.None; }
        }

    }
}