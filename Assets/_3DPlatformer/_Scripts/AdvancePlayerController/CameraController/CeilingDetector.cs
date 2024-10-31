using System;
using UnityEngine;

namespace Platformer.Advanced
{
    public class CeilingDetector : MonoBehaviour
    {
        public float ceilingAngleLimit = 10f;
        public bool isInDebugMode;
        private float debugDrawDuration = 2.0f;
        private bool ceilingWasHit;

        private void OnCollisionEnter(Collision other) => CheckForContact(other);

        private void OnCollisionStay(Collision other) => CheckForContact(other);

        private void CheckForContact(Collision other)
        {
            if (other.contacts.Length == 0) return;
            
            float angle = Vector3.Angle(-transform.up, other.contacts[0].normal);
            
            if(angle < ceilingAngleLimit)
                ceilingWasHit = true;
            if (isInDebugMode)
            {
                Debug.DrawRay(other.contacts[0].point, other.contacts[0].normal, Color.red,debugDrawDuration);
            }
        }
        
        public bool HitCeiling() => ceilingWasHit;
        public void Reset() =>ceilingWasHit = false;
    }
}