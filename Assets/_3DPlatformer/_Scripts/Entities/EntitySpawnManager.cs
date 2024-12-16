using UnityEngine;
using UnityEngine.Serialization;

namespace Platformer
{
    public abstract class EntitySpawnManager: MonoBehaviour
    {
        [SerializeField] protected SpawnPointStratgyType _spawnPointStratgyType = SpawnPointStratgyType.Linear;
        [SerializeField] protected Transform[] _spawnPoints;
        protected ISpawnPointStrategy _spawnPointStratgy;
       protected enum SpawnPointStratgyType
        {
            Linear,
            Random,
            Circle
        }

        protected virtual void Awake()
        {
            _spawnPointStratgy = _spawnPointStratgyType switch
            {
                SpawnPointStratgyType.Linear => new LinearSpawnPointStrategy(_spawnPoints),
                SpawnPointStratgyType.Random => new RandomSpawnPointStrategy(_spawnPoints),
                _ => _spawnPointStratgy
            };
        }

        public abstract void Spawn();
    }
}