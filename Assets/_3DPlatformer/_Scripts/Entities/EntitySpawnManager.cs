using UnityEngine;

namespace Platformer
{
    public abstract class EntitySpawnManager : MonoBehaviour
    {
        [SerializeField] protected SpawnPointStratgyType _spawnPointStratgyType = SpawnPointStratgyType.Linear;
        [SerializeField] protected Transform[] spawnPoints;
        
        protected ISpawnPointStrategy _spawnPointStratgy;
       protected enum SpawnPointStratgyType
        {
            Linear,
            Random
        }

        protected virtual void Awake()
        {
            _spawnPointStratgy = _spawnPointStratgyType switch
            {
                SpawnPointStratgyType.Linear => new LinearSpawnPointStrategy(spawnPoints),
                SpawnPointStratgyType.Random => new RandomSpawnPointStrategy(spawnPoints),
                _ => _spawnPointStratgy
            };
        }

        public abstract void Spawn();
    }
}