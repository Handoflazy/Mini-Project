using System;
using Character;
using Platformer.ScriptableObjectData;
using UnityEngine;

namespace Platformer.Systems.SpawnSystem
{
    public class FallCatcher : MonoBehaviour
    {
        [SerializeField] private PathSO leadToPath;
        [SerializeField] private PathStorageSO pathStorageS0;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                pathStorageS0.lastPathTaken = leadToPath;
                other.GetComponent<Damageable>().Kill();
            }
        }
    }
}