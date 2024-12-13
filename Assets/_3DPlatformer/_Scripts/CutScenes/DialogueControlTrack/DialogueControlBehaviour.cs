using Platformer.Dialogue;
using UnityEngine;
using UnityEngine.Playables;

namespace Platformer.Dialogue
{
    public class DialogueControlBehaviour: PlayableBehaviour
    {
        public DialogueLineSO dialogueLine;
        public bool hasToPause;

        public void DisplayDialogueLine()
        {
            //TODO: Interface with the DialogueManager and play the line of dialogue on screen
            Debug.Log(dialogueLine.Sentence);
            //TODO: Check if it has to pause the Timeline
        }
    }
}