using System;
using UnityEngine;

namespace Platformer.CutScenes.Dialogue
{
    public class PseudoDialogueSystem : Singleton<PseudoDialogueSystem>
    {
        private Action dialogueCompleteCallback;
        public static void ShowDialogue(PseudoDialogueSO dialogue, Action dialogueCompleteCallback)
        {
            Instance.dialogueCompleteCallback = dialogueCompleteCallback;

            // TODO play dialogue and listen for player input
            Debug.LogError("dialogue not implemented");
        }

        private void OnPlayerActionedDialogue()
        {
            dialogueCompleteCallback.Invoke();
        }

    }
}