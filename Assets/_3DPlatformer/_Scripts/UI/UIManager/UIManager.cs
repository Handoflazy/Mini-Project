using System;
using Platformer.Dialogue;
using Sirenix.OdinInspector;
using UnityEngine;
using Utilities.Event_System.EventChannel;

namespace Platformer.UI.UIManager
{
    public class UIManager: MonoBehaviour
    {
        [Header("Scene UI")]
        [SerializeField] private UIDialogueManager dialogueController = default;
        
        
        [Header("Gameplay")]
        [SerializeField] private InputReader inputReader = default;
        [SerializeField] private ActorSO mainProtagonist = default;


        [Header("Dialogue Events")] [SerializeField]
        private DialogueLineChannelSO openUIDialogueEvent = default;

        private void OnEnable()
        {
            openUIDialogueEvent.OnEventRaised += OpenUIDialogue;
        }

        private void OnDisable()
        {
            openUIDialogueEvent.OnEventRaised -= OpenUIDialogue;
        }

        public void OpenUIDialogue(string dialogueLine, ActorSO actor)
        {
            bool isProtagonistTalking = (actor == mainProtagonist);
            dialogueController.gameObject.SetActive(true);
            dialogueController.SetDialogue(dialogueLine, actor, isProtagonistTalking);
            //interactionPanel.gameObject.SetActive(false);
        }
        public void CloseUIDialogue()
        {
            //selectionHandler.Unselect();
            dialogueController.gameObject.SetActive(false);
            //onInteractionEndedEvent.RaiseEvent();
        }
    }
}