using UnityEngine;
using UnityEngine.Serialization;

namespace Platformer
{
    [CreateAssetMenu(fileName = "Default Player Data", menuName = "Player Data/Data")]
    public class PlayerData : ScriptableObject
    {
        [Header("Movement Settings ")]
        // Movements speed of the player
        public float MoveSpeed = 6f;
        // Rotation speed of the player
        public float RotationSpeed = 15f;
        // Time to smooth the speed changes
        public float smoothTime = 0.2f;

        [Header("Jump Settings")] [SerializeField]
        public float JumpMaxHeight;
        public float JumpTimeToApex;
        public float GravilityMultiplier = 2f;
        public float JumpCooldown = 1f;
        public float JumpForce = 3f; 
        
        
        [FormerlySerializedAs("DashForce")] [Header("Dash Settings")]
        public float DashSpeed = 2f;
    }
}