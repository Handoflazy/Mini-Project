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
        [FormerlySerializedAs("actorList")] [SerializeField] private List<ActorSO> actorsList;
        [SerializeField] private InputReader inputReader;

        [Header("Listener On")] [SerializeField]
        private DialogueDataChannelSO startDialogue;
        
        
        [FormerlySerializedAs("openDialogueEvent")]
        [Header("Broadcasting on")] 
        [SerializeField] private DialogueLineChannelSO openUIDialogueEvent = default;

        [SerializeField] private VoidEventChannel closeUIDialogueEvent = default;

        private int counterDialogue; // current line in conservation.
        private int counterLine; // current word 
        
        private DialogueDataSO currentDialogue = default;
        
        private bool ReachEndOfDialogue => counterDialogue >= currentDialogue.Conversation.Count;
        private bool ReachedEndOfLine => counterLine >= currentDialogue.Conversation[counterDialogue].Sentence.Length;

        private void Start()
        {
            startDialogue.OnEventRaised += DisplayDialogueData;
        }

        public void DisplayDialogueData(DialogueDataSO dialogueDataSO)
        {
            counterDialogue = 0;
            counterLine = 0;
            inputReader.EnableDialogueInput();
            inputReader.AdvanceDialogueEvent += OnAdvance;
            currentDialogue = dialogueDataSO;
            if (currentDialogue.Conversation != null)
            {
                ActorSO currentActor =
                    actorsList.Find(o => o.ActorId == currentDialogue.Conversation[counterDialogue].Actor.ActorId);
                DisplayDialogueLine(currentDialogue.Conversation[counterDialogue].Sentence,currentActor);
            }
            else
            {
                Debug.LogError("Check Dialogue");
            }
        }

        private void OnAdvance()
        {
            /*counterLine++;
            if (!ReachedEndOfLine)
            {
                ActorSO currentActor = actorsList.Find(o => o.ActorId == currentDialogue.Conversation[counterDialogue].Actor.ActorId);
                DisplayDialogueLine(currentDialogue.Conversation[counterDialogue].Sentence, currentActor);
            }
            else
            {
                counterDialogue++;
                if (!ReachEndOfDialogue)
                {
                    counterLine = 0;
                    ActorSO currentActor = actorsList.Find(o => o.ActorId == currentDialogue.Conversation[counterDialogue].Actor.ActorId);
                    DisplayDialogueLine(currentDialogue.Conversation[counterDialogue].Sentence, currentActor);
                    
                }
                else
                {
                    DialogueEndedAndCloseDialogueUI();
                }
            }*/
            counterDialogue++;
            if (!ReachEndOfDialogue)
            {
                counterLine = 0;
                ActorSO currentActor = actorsList.Find(o => o.ActorId == currentDialogue.Conversation[counterDialogue].Actor.ActorId);
                DisplayDialogueLine(currentDialogue.Conversation[counterDialogue].Sentence, currentActor);
                    
            }
            else
            {
                DialogueEndedAndCloseDialogueUI();
            }
            
        }

        public void DisplayDialogueLine(string dialogueLine, ActorSO actor)
        {
            openUIDialogueEvent.RaiseEvent(dialogueLine, actor);
        }
        private void DialogueEndedAndCloseDialogueUI()
        {
            inputReader.AdvanceDialogueEvent -= OnAdvance;
            inputReader.EnableGameplayInput();
            closeUIDialogueEvent.Invoke();
        }
    }
   
}