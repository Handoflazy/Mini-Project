using Utilities.ImprovedTimers;
using Unity.VisualScripting;
using UnityEngine;
using Timer = Unity.VisualScripting.Timer;

namespace Platformer
{
    public class ConeDetectionStragedy : IDectionStragedy
    {
        private readonly float detectionRadius;
        private readonly float detectionAngle;
        private readonly float innerDetectionRadius;
        public ConeDetectionStragedy(float detectionRadius, float detectionAngle, float innerDetectionRadius)
        {
            this.detectionRadius = detectionRadius;
            this.detectionAngle = detectionAngle;
            this.innerDetectionRadius = innerDetectionRadius;
        }

        public bool Execute(Transform player, Transform detector, CountdownTimer detectionTimer)
        {
            if (detectionTimer.IsRunning)
                return false;
            Vector3 directionToPlayer = player.position - detector.transform.position;
            float anglePlayer = Vector3.Angle(detector.forward, directionToPlayer);
            if (!(directionToPlayer.magnitude <= detectionRadius && anglePlayer < detectionAngle / 2f&& !(directionToPlayer.magnitude < innerDetectionRadius)))
                return false;
            detectionTimer.Start();
            return true;
        }
    }
}