using UnityEngine;

namespace Platformer
{
    public class EntitySpawner<T> where T : Entity
    {
        private ISpawnPointStrategy _spawnPointStrategy;
        private IEntityFactory<T> _entiryFactory;

        public EntitySpawner(ISpawnPointStrategy spawnPointStrategy, IEntityFactory<T> entiryFactory)
        {
            _spawnPointStrategy = spawnPointStrategy;
            _entiryFactory = entiryFactory;
        }

        public T Spawn()
        {
            return _entiryFactory.Create(_spawnPointStrategy.NextSpawnPoint());
        }
    }
}