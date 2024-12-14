using UnityEngine;

namespace Platformer.Dialogue
{
    public enum ActorID 
    {
        MH, //Male HERO
        FH,// Female Hero
        VC// Villager Chef
    }
    /// <summary>
    /// Scriptable Object that represents an "Actor", that is the protagonist of a Dialogue
    /// </summary>
    [CreateAssetMenu(fileName = "newActor", menuName = "Dialogues/Actor")]
    public class ActorSO : ScriptableObject
    {
        [SerializeField] private ActorID actorId = default;
        [SerializeField] private string _actorName;
        
        public string ActorName { get => _actorName; }
        public ActorID ActorId => actorId;

        //[SerializeField] private Sprite _face; // dialogue is not have portait now
    }
}