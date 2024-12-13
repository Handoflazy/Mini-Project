using UnityEngine;

namespace Platformer.Dialogue
{
    [CreateAssetMenu(menuName = "CutsceneSystem/Actor")]
    public class Actor : ScriptableObject
    {
        public string ActorName { get => _actorName; }
        [SerializeField] private string _actorName;
        //[SerializeField] private Sprite _face; // dialogue is not have portait now
    }
}