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
            isPause = false;
            
            playableDirector = activePlayableDirector;
            playableDirector.Play();
            playableDirector.stopped += ctx => CutsceneEnded();
        }

        public void PlayDialogueFromClip(string dialogueLine, ActorSO actor)
        {
            dialogueManager.DisplayDialogueLine(dialogueLine,actor );
        }

        void OnAdvance()
        {   
            if(isPause)
                ResumeTimeline();
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

        void CutsceneEnded()
        {
            inputReader.EnableGameplayInput();
        }
    }
}