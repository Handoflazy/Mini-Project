using System;
using System.Collections.Generic;
using Platformer.Dialogue;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;
using Utilities.Event_System.EventChannel;

namespace Platformer.CutScenes
{
    /// <summary>
    /// <para>Takes care of all things dialogue, whether they are coming from within a Timeline or just from the interaction with a character, or by any other mean.</para>
    /// <para>Keeps track of choices in the dialogue (if any) and then gives back control to gameplay when appropriate.</para>
    /// </summary>
    public class DialogueManager : MonoBehaviour
    {
        [SerializeField] private List<ActorSO> actorsList;
        [SerializeField] private InputReader inputReader;

        [Header("Listener On")] 
        [SerializeField] private DialogueDataChannelSO startDialogue;
        
        [Header("Broadcasting on")] 
        [SerializeField] private DialogueLineChannelSO openUIDialogueEvent = default;
        [SerializeField] private VoidEventChannel closeUIDialogueEvent = default;

        private int counterDialogue; // current line in conservation.
        
        private DialogueDataSO currentDialogue = default;
        private bool ReachEndOfDialogue => counterDialogue >= currentDialogue.Lines.Count;

        private void Start()
        {
            startDialogue.OnEventRaised += DisplayDialogueData;
        }

        private void OnDisable()
        {
            startDialogue.OnEventRaised -= DisplayDialogueData;
        }

        public void DisplayDialogueData(DialogueDataSO dialogueDataSO)
        {
            counterDialogue = 0;
            inputReader.EnableDialogueInput();
            inputReader.AdvanceDialogueEvent += OnAdvance;
            currentDialogue = dialogueDataSO;
            
            if (currentDialogue.Lines != null)
            {
                ActorSO currentActor =
                    actorsList.Find(o => o.ActorId == currentDialogue.Lines[counterDialogue].Actor.ActorId);
                DisplayDialogueLine(currentDialogue.Lines[counterDialogue].Sentence,currentActor);
            }
            else
            {
                Debug.LogError("Check Dialogue");
            }
        }

        private void OnAdvance()
        {
            counterDialogue++;
            if (!ReachEndOfDialogue)
            {
                ActorSO currentActor = actorsList.Find(o => o.ActorId == currentDialogue.Lines[counterDialogue].Actor.ActorId);
                DisplayDialogueLine(currentDialogue.Lines[counterDialogue].Sentence, currentActor);
                    
            }
            else
            {
                DialogueEndedAndCloseDialogueUI();
            }
            
        }
        

        public void DisplayDialogueLine(string dialogueLine, ActorSO actor, float charactersPerSecond = -1)
        {
            openUIDialogueEvent.RaiseEvent(dialogueLine, actor);
        }
        private void DialogueEndedAndCloseDialogueUI()
        {
            currentDialogue.FinishDialogue();
            
            inputReader.AdvanceDialogueEvent -= OnAdvance;
            inputReader.EnableGameplayInput();
            closeUIDialogueEvent.Invoke();
        }

        public void CutsceneDialogueEnded()
        {
            closeUIDialogueEvent.Invoke();
        }
    }
   
}