/*using System;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

namespace Platformer.CutScenes
{
    [RequireComponent(typeof(PlayableDirector))]
    public class CutsceneManager:Singleton<CutsceneManager>
    {
        public bool IsInteracting { get; private set; }
        public int DialogueCounter { get; private set; }
        public PlayableDirector Director { get; private set; }
        
        [SerializeField] private InputReader _inputReader= default;
        private CutsceneData _cutsceneData;
        public void Play(CutsceneData cutsceneData)
        {
            IsInteracting = true;

            _cutsceneData = cutsceneData;

           

            Interacting(); 
        }
        public void PauseTimeline()
        {
            director.playableGraph.GetRootPlayable(0).SetSpeed(0); 
        }

        public void ResumeTimeline()
        {
            director.playableGraph.GetRootPlayable(0).SetSpeed(1);
        }
        public void AbleToInteract(Vector3 posInWorld, CutsceneData cutsceneData)
        {
            IsInteracting = true;
            ShowInteractionBox(posInWorld);
            EnableInteractionInput();
            _cutsceneData = cutsceneData;
        }

        private void ShowInteractionBox(Vector3 posInWorld)
        {
            throw new NotImplementedException();
        }

        public void NotAbleToInteract()
        {
            EnableGameplayInput();
            CloseDialogueBox();
            IsInteracting = false;
            _dialogueCounter = 0;
            DisableInteractionBox();
        }

        private void DisableInteractionBox()
        {
            throw new NotImplementedException();
        }

        private void OnAdvance()
        {
            if(_dialogueCounter == 0)
            {
                Play(_cutsceneData);
            }

            if (_normalDialogueBox.Box.activeInHierarchy)
            {
                if (_dialogueCounter < _cutsceneData.dialogueDataSo.Conversation.Count - 1)
                {
                    AdvanceDialogueBox();
                }
                else
                {
                    NotAbleToInteract();
                }
            } 
        }
        public void Interacting()
        {
            DisableInteractionBox();

            DisableGameplayInput(); 
        } 
        private void EnableInteractionInput()
        {
            _inputReader.EnableMenusInput();
        }
        private void DisableGameplayInput()
        {
            _inputReader.EnableDialogueInput();
        }

        /// <summary>
        /// Enable Gameplay input and disable menus input.
        /// </summary>
        private void EnableGameplayInput()
        {
            _inputReader.EnableGameplayInput();
        }
        private void AdvanceDialogueBox()
        {
            _dialogueCounter++;
            UpdateDialogueBox();
        }

        private void CloseDialogueBox()
        {
            _normalDialogueBox.Box.SetActive(false);
        }
        private void UpdateDialogueBox()
        {
            
        }
        private void OnCutsceneCompleted(PlayableDirector director)
        {
            director.stopped -= OnCutsceneCompleted;

            // TODO return to normal gameplay
        }
        
    }
    [Serializable]
    public class DialogueBox
    {
        public GameObject Box;
        public TMP_Text Name;
        public TMP_Text Message;
        public Image Image;
    }
}*/