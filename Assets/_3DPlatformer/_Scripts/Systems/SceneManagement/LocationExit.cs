using System;
using Platformer.ScriptableObjectData;
using UnityEngine;
using Utilities.EventChannel;

namespace Platformer.Systems.SpawnSystem
{
    public class LocationExit : MonoBehaviour
    {
        [SerializeField] private int sceneGroupToLoad = default;
        [SerializeField] private PathSO leadToPath;
        [SerializeField] private PathStorageSO pathStorage;

        [Header("Broadcasting on")] [SerializeField]
        private IntEventChannel loadSceneIndex;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                pathStorage.lastPathTaken = leadToPath;
                loadSceneIndex.Invoke(sceneGroupToLoad);
            }
        }
    }
}