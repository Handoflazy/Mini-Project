using System;
using Utilities.ImprovedTimers;
using Character;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Platformer
{
    public class PlayerDetector : MonoBehaviour
    {
       
        [SerializeField, Range(0,5)] private float innerDetectionRadius = 0.5f;
        [SerializeField, Range(0, 360)] private float detectionAngle = 30f;
        [SerializeField, Range(0, 1)] private float detectionCooldown = 0.2f;
        [SerializeField, Range(0, 20)] private float detectionRadius = 10f;
        public float attackRange = 5f;
        
        public Transform player;
        
        
        
        private IDectionStragedy detectionStragedy;

        private CountdownTimer detectionTimer;
        public Damageable PlayerDamageable { get; private set; }
        
        

        public void SetUp(Transform playerTransform)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
            if (player)
            {
                PlayerDamageable = player.GetComponent<Damageable>();
            }
        }

        private void Start()
        {
            detectionTimer = new CountdownTimer(detectionCooldown);
            detectionStragedy = new ConeDetectionStragedy(detectionRadius, detectionAngle, innerDetectionRadius);
        }
        public bool CanDetectPlayer() {
            return detectionTimer.IsRunning || detectionStragedy.Execute(player, transform, detectionTimer);
        }

        public bool CanAttackPlayer()
        {
            return Vector3.Distance(player.position, transform.position) <= attackRange;
        }
        void OnDrawGizmos() {
            Gizmos.color = Color.red;

            // Draw a spheres for the radii
            Gizmos.DrawWireSphere(transform.position, detectionRadius);
            Gizmos.DrawWireSphere(transform.position, innerDetectionRadius);

            // Calculate our cone directions
            Vector3 forwardConeDirection = Quaternion.Euler(0, detectionAngle / 2, 0) * transform.forward * detectionRadius;
            Vector3 backwardConeDirection = Quaternion.Euler(0, -detectionAngle / 2, 0) * transform.forward * detectionRadius;

            // Draw lines to represent the cone
            Gizmos.DrawLine(transform.position, transform.position + forwardConeDirection);
            Gizmos.DrawLine(transform.position, transform.position + backwardConeDirection);
            
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position,attackRange);
        }
    }
}