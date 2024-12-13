using Platformer.Dialogue;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Platformer.CutScenes
{
    /// <summary>
    /// <para>Takes care of all things dialogue, whether they are coming from within a Timeline or just from the interaction with a character, or by any other mean.</para>
    /// <para>Keeps track of choices in the dialogue (if any) and then gives back control to gameplay when appropriate.</para>
    /// </summary>
    public class DialogueManager : MonoBehaviour
    {
        [SerializeField] private InputReader inputReader;
        public DialogueDataSO testDialogue;
        [Button]
        public void startTest()
        {
            BeginDialogue(testDialogue.Conversation[0]);
        }
        public void BeginDialogue(DialogueLineSO firstLine)
        {
            inputReader.EnableDialogueInput();
            DisplayDialogueLine(firstLine);
        }

        public void DisplayDialogueLine(DialogueLineSO dialogueLine)
        {
            print(dialogueLine.Sentence);
        }
    }
   
}