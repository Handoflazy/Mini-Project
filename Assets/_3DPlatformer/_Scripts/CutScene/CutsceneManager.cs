using System;
using Platformer.Dialogue;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Platformer.CutScenes
{
    public class CutsceneManager : MonoBehaviour
    {
        [FormerlySerializedAs("activePlayableDirector")] [SerializeField,Required]
        private PlayableDirector playableDirector;
        
        [SerializeField] private InputReader inputReader = default;
        [SerializeField] private DialogueManager dialogueManager = default;
        
        
        //can't use playableGraph.IsPlaying cause we have Pause state while playing;
        public bool IsCutscenePlaying => playableDirector.playableGraph.GetRootPlayable(0).GetSpeed() != 0d;

        private void HandleDirectorStopped(PlayableDirector director) => CutsceneEnded();

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
            inputReader.EnableDialogueInput();
            playableDirector = activePlayableDirector;
            
            isPause = false;
            playableDirector.Play();
            playableDirector.stopped += HandleDirectorStopped;
        }

        void OnAdvance()
        {
            if (isPause)
            {
                LineEnded();
                ResumeTimeline();
            }
        }

        public void LineEnded()
        {
            dialogueManager.CutsceneDialogueEnded();
        }
        void CutsceneEnded()
        {
            if (playableDirector != null)
            {
                playableDirector.stopped -= HandleDirectorStopped;
            }
            dialogueManager.CutsceneDialogueEnded();
            inputReader.EnableGameplayInput();
        }
        
        public void PauseTimeline()
        {
            isPause = true;
            playableDirector.playableGraph.GetRootPlayable(0).SetSpeed(0);
        }

        void ResumeTimeline()
        {
            isPause = false;
            playableDirector.playableGraph.GetRootPlayable(0).SetSpeed(1);
        }

    }
}