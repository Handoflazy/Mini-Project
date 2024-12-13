using Platformer.CutScenes;
using Platformer.Dialogue;
using UnityEngine;
using UnityEngine.Playables;

namespace Platformer.Dialogue
{
    public class DialogueBehaviour: PlayableBehaviour
    {
        public DialogueLineSO dialogueLine = default;
        public bool pauseWhenClipEnd = default;//This won't work if the clip ends on the very last frame of the Timeline
        
        [HideInInspector] public CutsceneManager cutsceneManager;
        private bool dialoguePlayed;
        public override void OnBehaviourPlay(Playable playable, FrameData info)
        {
            if(dialoguePlayed) return;
            if (playable.GetGraph().IsPlaying() && cutsceneManager.IsCutscenePlaying)
            {
                if(dialogueLine != null)
                {
                    cutsceneManager.PlayDialogueFromClip(dialogueLine);
                    dialoguePlayed = true;
                }
                else
                {
                    Debug.LogWarning("This clip contains no DialogueLine");
                }
            }
        }

        public override void OnBehaviourPause(Playable playable, FrameData info)
        {
            // The check on _dialoguePlayed is needed because OnBehaviourPause is called also at the beginning of the Timeline,
            // so we need to make sure that the Timeline has actually gone through this clip (i.e. called OnBehaviourPlay) at least once before we stop it
            if (Application.isPlaying
                && playable.GetGraph().IsPlaying()
                && !playable.GetGraph().GetRootPlayable(0).IsDone()
                && dialoguePlayed)
            {
                if(pauseWhenClipEnd)
                    cutsceneManager.PauseTimeline();
            }
        }
    }
}