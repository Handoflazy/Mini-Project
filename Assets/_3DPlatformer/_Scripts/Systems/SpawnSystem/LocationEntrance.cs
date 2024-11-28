using Platformer.ScriptableObjectData;
using Unity.VisualScripting;
using UnityEngine;

namespace Platformer.Systems.SpawnSystem
{
    public class LocationEntrance : MonoBehaviour
    {
        [SerializeField] private PathStorageSO pathStorage;
        [SerializeField] private PathSO entrancePath;

        public PathSO EntrancePath => entrancePath;

        private void Awake()
        {
            if (pathStorage.lastPathTaken == entrancePath)
            {
                
            }
        }

        public void PlanTransition()
        {
            //TODO: IdontKnow;
        }
    }
}