using System;
using System.Linq;
using AdvancePlayerController;
using Platformer.ScriptableObjectData;
using Unity.VisualScripting;
using UnityEngine;
using Utilities.EventChannel;

namespace Platformer.Systems.SpawnSystem
{
    public class SpawnSystem : MonoBehaviour
    {
        [Header("Asset Reference")]
        [SerializeField] private InputReader input;
        [SerializeField] private Protagonist playerPrefab;
        [SerializeField] private TransformEventChannel playerInstantiatedChannel;
        [SerializeField] private PathStorageSO pathTanken;
        
        private Transform defaultSpawnPoint;
        private LocationEntrance[] spawnLocation;
        
        private void Awake()
        {
            spawnLocation = FindObjectsOfType<LocationEntrance>();
            defaultSpawnPoint = transform.GetChild(0);
        }

        public void SpawnPlayer()
        {
            Transform location = GetSpawnLocation();
            Protagonist playerInstance = Instantiate(playerPrefab, location.position, location.rotation);
            
            playerInstantiatedChannel.Invoke(playerInstance.transform);
            input.EnableGameplayInput();
        }

        private Transform GetSpawnLocation()
        {
            if (pathTanken == null)
                return defaultSpawnPoint;
            int entranceIndex =
                Array.FindIndex(spawnLocation,
                    element => element.EntrancePath == pathTanken.lastPathTaken);
            if (entranceIndex == -1)
            {
                return defaultSpawnPoint;
            }

            return spawnLocation[entranceIndex].transform;
        }
    }
}