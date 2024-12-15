using System;
using Platformer.Dialogue;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Utilities.EventChannel;

namespace Platformer.CutScenes
{
    public class CutsceneManager : MonoBehaviour
    {
        [SerializeField] private InputReader inputReader = default;
        [SerializeField] private DialogueManager dialogueManager = default;

        [Header("Listening On")] 
        [SerializeField] private PlayableDirectorChannelSO playCutSceneEvent = default;
        [SerializeField] private DialogueLineChannelSO playDialogueLineEvent = default;
        
        
        private PlayableDirector playableDirector;
        
        //can't use playableGraph.IsPlaying cause we have Pause state while playing;
        public bool IsCutscenePlaying => playableDirector.playableGraph.GetRootPlayable(0).GetSpeed() != 0d;

        private void HandleDirectorStopped(PlayableDirector director) => CutsceneEnded();

        private bool isPause;

        private void OnEnable()
        {
            inputReader.AdvanceDialogueEvent +=OnAdvance;
            playCutSceneEvent.OnEventRaised += PlayCutscene;
        }

        private void OnDisable()
        {
            inputReader.AdvanceDialogueEvent -=OnAdvance;
            playCutSceneEvent.OnEventRaised -= PlayCutscene;
            playDialogueLineEvent.OnEventRaised -= PlayDialogueFromClip;
        }

        private void Start()
        {
            playCutSceneEvent.OnEventRaised += PlayCutscene;
            playDialogueLineEvent.OnEventRaised += PlayDialogueFromClip;
        }

        private void PlayDialogueFromClip(string dialogueLine, ActorSO actor)
        {
            dialogueManager.DisplayDialogueLine(dialogueLine, actor);
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