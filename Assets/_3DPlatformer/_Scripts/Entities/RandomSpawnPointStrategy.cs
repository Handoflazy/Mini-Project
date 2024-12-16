using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Platformer
{
    public class RandomSpawnPointStrategy : ISpawnPointStrategy
    {
        List<Transform> _unusedSpawnPoints;
        private Transform[] _spawnPoints;

        public RandomSpawnPointStrategy(Transform[] spawnPoints)
        {
            _spawnPoints = spawnPoints;
            _unusedSpawnPoints = new List<Transform>(spawnPoints);
        }
        public Transform NextSpawnPoint()
        {
            if (!_unusedSpawnPoints.Any())
            {
                _unusedSpawnPoints = new List<Transform>(_spawnPoints);
            }
            var randomIndex = Random.Range(0, _unusedSpawnPoints.Count);
            var result = _unusedSpawnPoints[randomIndex];
            _unusedSpawnPoints.RemoveAt(randomIndex);
            return result;
        }
    }
    
}