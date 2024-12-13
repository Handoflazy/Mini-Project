using System;
using Platformer.Dialogue;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Platformer.CutScenes
{
    public class CutsceneManager : MonoBehaviour
    {
        private PlayableDirector activePlayableDirector;
        
        [SerializeField] private InputReader inputReader = default;
        [SerializeField] private DialogueManager dialogueManager = default;
        
        //can't no use playableGraph.IsPlaying cause we have Pause state while playing;
        public bool IsCutscenePlaying => activePlayableDirector.playableGraph.GetRootPlayable(0).GetSpeed() != 0d;

        private bool isPause;

        private void OnEnable()
        {
            inputReader.AdvanceDialogueEvent +=OnAdvance;
        }

        private void OnDisable()
        {
            inputReader.AdvanceDialogueEvent -=OnAdvance;
        }

        public void PlayCutscene(PlayableDirector activePlayableDirector)
        {
            isPause = false;
            this.activePlayableDirector = activePlayableDirector;
            activePlayableDirector.Play();
            activePlayableDirector.stopped += ctx => CutsceneEnded();
            inputReader.EnableDialogueInput();
        }

        public void PlayDialogueFromClip(DialogueLineSO dialogueLine)
        {
            dialogueManager.DisplayDialogueLine(dialogueLine);
        }

        void OnAdvance()
        {   
            if(isPause)
                ResumeTimeline();
        }

        public void PauseTimeline()
        {
            isPause = true;
            activePlayableDirector.playableGraph.GetRootPlayable(0).SetSpeed(0);
        }

        public void ResumeTimeline()
        {
            isPause = false;
            activePlayableDirector.playableGraph.GetRootPlayable(0).SetSpeed(1);
        }

        void CutsceneEnded()
        {
            inputReader.EnableGameplayInput();
        }
    }
}