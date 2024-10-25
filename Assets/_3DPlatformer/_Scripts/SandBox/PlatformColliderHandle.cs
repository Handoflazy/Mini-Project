using System;
using UnityEngine;

namespace Platformer
{
    public class PlatformColliderHandle : MonoBehaviour
    {
        private Transform _platform;

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag("MovingPlatform"))
            {
                
                ContactPoint contact = other.GetContact(0);
                Debug.DrawRay(contact.point, contact.normal, Color.red,   
                    1f);
                if (contact.normal.y < 0.5f)
                    return;
                _platform = other.transform;
                transform.SetParent(_platform);
            }
        }

        private void OnCollisionExit(Collision other)
        {
            if (other.gameObject.CompareTag("MovingPlatform"))
            {
                _platform = null;
                transform.SetParent(null);
            }
        }
    }
}