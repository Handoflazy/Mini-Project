using System;
using UnityEngine;
using UnityEngine.Events;

namespace Platformer
{
    public class Collectible : Entity
    {
        public UnityEvent OnCollect;
        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                Destroy(gameObject);
                OnCollect?.Invoke();
            }
        }

      
    }
}