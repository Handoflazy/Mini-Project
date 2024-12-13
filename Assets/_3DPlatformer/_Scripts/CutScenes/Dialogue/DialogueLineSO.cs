using UnityEngine;

namespace Platformer.Dialogue
{
    [CreateAssetMenu(fileName = "newLineOfDialogue", menuName = "Dialogues/Line of Dialogue")]
    public class DialogueLineSO: ScriptableObject
    {
        public Actor Actor => _actor;
        public string Sentence => _sentence;
        [SerializeField] private Actor _actor = default;
        [SerializeField] [TextArea(3, 3)] private string _sentence = default;
    }
}