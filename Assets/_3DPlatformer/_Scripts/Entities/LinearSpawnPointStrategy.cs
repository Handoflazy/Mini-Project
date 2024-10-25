using UnityEngine;

namespace Platformer
{
    public class LinearSpawnPointStrategy : ISpawnPointStrategy
    {
        private int _index = 0;
        private Transform[] _spawnPoints;

        public LinearSpawnPointStrategy(Transform[] spawnPoints)
        {
            _spawnPoints = spawnPoints;
        }
        public Transform NextSpawnPoint()
        {
            Transform result = _spawnPoints[_index];
            _index =(_index + 1) % _spawnPoints.Length;
            return result;
        }
    }
}