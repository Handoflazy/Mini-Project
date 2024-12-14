using System;
using Platformer.CutScenes;
using Platformer.Dialogue;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Serialization;
using Utilities.Event_System.EventChannel;

namespace Platformer.Dialogue
{
    [Serializable]
    public class DialogueBehaviour: PlayableBehaviour
    {
        public DialogueLineSO dialogueLine = default;
        [SerializeField] private ActorSO actor = default;
        [SerializeField] private bool PauseWhenClipEnd = default;//This won't work if the clip ends on the very last frame of the Timeline

        [HideInInspector] public DialogueLineChannelSO PlayDialogueEvent;
        [HideInInspector] public VoidEventChannel PauseTimeLineEvent;
        
        private bool dialoguePlayed;
        
        /// <summary>
        /// Displays a line of dialogue on screen by interfacing with the <c>CutsceneManager</c>. 
        /// </summary>
        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            if(dialoguePlayed) return;
            // Need to ask the CutsceneManager if the cutscene is playing, since the graph is not actually stopped/paused: it's just going at speed 0
            // This check is needed because when two clips are side by side and the first one stops the cutscene,
            // the OnBehaviourPlay of the second clip is still called and thus its dialogueLine is played (prematurely). This check makes sure it's not.
            if (!Application.isPlaying) return; //TODO: Find a way to "play" dialogue lines even when scrubbing the Timeline not in Play Mode
            if (!playable.GetGraph()
                    .IsPlaying()) return; //&& cutsceneManager.IsCutscenePlaying) Need to find an alternative to this, now noctice to decoupe two dialogue clip
            if (dialogueLine != null)
            {
                PlayDialogueEvent?.RaiseEvent(dialogueLine.Sentence, dialogueLine.Actor);
                dialoguePlayed = true;
            }
            else
            {
                Debug.LogWarning("This clip contains no DialogueLine");
            }
        }
        /// <summary>
        /// Called when the clip becomes deactivated. This occurs when the timeline starts, when the clip is passed it’s duration, or if the timeline is stopped.
        /// </summary>
        public override void OnBehaviourPause(Playable playable, FrameData info)
        {
            // The check on _dialoguePlayed is needed because OnBehaviourPause is called also at the beginning of the Timeline,
            // so we need to make sure that the Timeline has actually gone through this clip (i.e. called OnBehaviourPlay) at least once before we stop it
            if (Application.isPlaying
                && playable.GetGraph().IsPlaying()
                && !playable.GetGraph().GetRootPlayable(0).IsDone()
                && dialoguePlayed)
            {
                if(PauseWhenClipEnd)
                    PauseTimeLineEvent.Invoke();
            }
        }
    }
}