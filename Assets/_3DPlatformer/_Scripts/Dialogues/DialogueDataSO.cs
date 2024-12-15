using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Timeline;
using Utilities.EventChannel;

namespace Platformer.Dialogue
{
    public enum DialogueType
    {
        StartDialogue,
        CompletionDialogue,
        IncompletionDialogue,
        DefaultDialogue,
    }
    
    /// <summary>
    /// A Dialogue is a list of consecutive DialogueLines. They play in sequence using the input of the player to skip forward.
    /// In future versions it might contain support for branching conversations.
    /// </summary>
    [CreateAssetMenu(fileName = "newDialogue", menuName = "CutsceneSystem/Dialogue Data")]
    public class DialogueDataSO : ScriptableObject
    {
        [SerializeField] private List<DialogueLineSO> lines;
        [SerializeField] private VoidEventChannel endOfDialogueEvent = default;
        [SerializeField] private DialogueType dialogueType;
        
        public List<DialogueLineSO> Lines => lines;
        public VoidEventChannel EndOfDialogueEvent => endOfDialogueEvent;
        public DialogueType DialogueType
        {
            get { return dialogueType; }
            set { dialogueType = value; }
        }
        public void FinishDialogue()
        {
            if (EndOfDialogueEvent != null)
                EndOfDialogueEvent.Invoke();
        }
    }
}