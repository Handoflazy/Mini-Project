using UnityEngine;

namespace Platformer
{

    public class EntityFactory<T> : IEntityFactory<T> where T : Entity
    {
        private EntityData[] _data;

        public EntityFactory(EntityData[] data)
        {
            _data = data;
        }
        public T Create(Transform spawnPoint)
        {
            EntityData entityData = _data[Random.Range(0, _data.Length)];
            GameObject instance = GameObject.Instantiate(entityData.Prefab,spawnPoint.position, Quaternion.identity);
            return instance.GetComponent<T>();
        }
    }
    
}
