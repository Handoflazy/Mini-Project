using System;
using UnityEngine;
using Utilities;

namespace Platformer
{
    public class CollectibleSpawnManager : EntitySpawnManager
    {
        [SerializeField] CollectibleData[] collectibleData;
        [SerializeField] float spawnInterval =  1f;
        EntitySpawner<Collectible> spawner;

        private CooldownTimer _spawnTimer;
        private int _counter = 0;
        protected override void Awake()
        {
            base.Awake();
            spawner = new EntitySpawner<Collectible>(_spawnPointStratgy, new EntityFactory<Collectible>(collectibleData));
            _spawnTimer = new CooldownTimer(spawnInterval);
            
            _spawnTimer.OnTimeStop += () =>
            {
                if (_counter >= _spawnPoints.Length)
                {
                    _spawnTimer.Stop();
                    return;
                }
                Spawn();
                _counter++;
                _spawnTimer.Start();
            };
        }

        private void Update()
        {
            _spawnTimer.Tick(Time.deltaTime);
        }

        private void Start() => _spawnTimer.Start();

        public override void Spawn() => spawner.Spawn();
    }
}