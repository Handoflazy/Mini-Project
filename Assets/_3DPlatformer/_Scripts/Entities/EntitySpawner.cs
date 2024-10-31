using UnityEngine;

namespace Platformer
{
    public class EntitySpawner<T> where T : Entity
    {
        private ISpawnPointStrategy _spawnPointStrategy;
        private IEntityFactory<T> _entityFactory;

        public EntitySpawner(ISpawnPointStrategy spawnPointStrategy, IEntityFactory<T> entityFactory)
        {
            _spawnPointStrategy = spawnPointStrategy;
            _entityFactory = entityFactory;
        }

        public T Spawn()
        {
            return _entityFactory.Create(_spawnPointStrategy.NextSpawnPoint());
        }
    }
}