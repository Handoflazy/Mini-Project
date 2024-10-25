using UnityEngine;

namespace Platformer
{
    public interface IEntityFactory<T>
    {
        T Create(Transform spawnPoint);
    }
}