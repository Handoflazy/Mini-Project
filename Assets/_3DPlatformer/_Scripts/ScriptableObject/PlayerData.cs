using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace Platformer._Scripts.ScriptableObject
{
    [CreateAssetMenu(menuName = "Player Data")]
    public class PlayerData : UnityEngine.ScriptableObject
    {
        #region Movement

        [TabGroup("Movement", "Run")]
        public float RunMaxSpeed = 8f;
        [TabGroup("Movement", "Air"),Range(0,1)]
        public float AirControlRate = 0.5f;
        [TabGroup("Movement", "Jump")]
        public float JumpHeight = 4f;
        [TabGroup("Movement", "Jump")]
        public float JumpTimeToApex = 0.4f;
        [TabGroup("Movement", "Sprint")]
        public float RunMultiplier = 1.5f;
        [TabGroup("Movement", "Slide")]
        public float SlideToJumpThreshold;
        #endregion

        #region Gravity

        [TabGroup("Physics", "Gravity")]
        public float Gravity = 9.81f;
        [TabGroup("Physics", "Gravity")]
        public float FallGravityMult = 1.5f;
         [TabGroup("Physics", "Gravity")]
        public float SlideGravity = 20f;
        [TabGroup("Physics", "Gravity")]
        public float MaxFallDistance = 8;
        

        #endregion

        #region Other

        [TabGroup("Physics", "Other")]
        public float SlopeLimit = 30f;
        [TabGroup("Physics", "Other")]
        [SerializeField, Range(0f, 50f)]
        public float GroundFriction = 8f;
        [TabGroup("Physics", "Other")]
        [ Range(0f, 50f)]
        public float AirFriction = 0.2f;

        [TabGroup("Assists")]
        [Range(0.01f, 0.5f)]
        public float CoyoteTime = 0.15f;
        [TabGroup("Assists")]
        [Range(0.01f, 0.5f)]
        public float JumpInputBufferTime = 0.15f;
        #endregion

        #region Readonly Properties

        [TabGroup("Physics", "Gravity")]
        [ShowInInspector, ReadOnly]
        public float GravityStrength { get; private set; }

        [TabGroup("Physics", "Gravity")]
        [ShowInInspector, ReadOnly]
        public float GravityScale { get; private set; }
        [TabGroup("Movement", "Jump")]
        [ShowInInspector, ReadOnly]
        public float JumpForce { get; private set; }

        #endregion

        #region Time for Timer
        public float RunCooldownTime = 2f;
        public float RunTime = 3f;
        public float SurprisedAnimationTime =1f;

        #endregion

        [OnInspectorInit]
        private void OnValidate()
        {
            GravityStrength = (2 * JumpHeight) / (JumpTimeToApex * JumpTimeToApex);
            GravityScale = GravityStrength / Gravity;
            JumpForce = Mathf.Abs(GravityStrength) * JumpTimeToApex;
            
        }
    }
}