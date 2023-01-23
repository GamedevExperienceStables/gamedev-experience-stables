using Game.Actors;
using Game.Level;
using Game.Stats;
using UnityEngine;

namespace Game.Enemies
{
    [RequireComponent(typeof(NavigationController))]
    public class EnemyController : ActorController
    {
        public override IStatsSet Stats => _stats;

        private AiController _ai;
        private MovementController _movement;
        
        private readonly EnemyStats _stats = new();
        private LootController _loot;

        protected override void OnAwake()
        {
            _ai = GetComponent<AiController>();
            _movement = GetComponent<MovementController>();
            _loot = GetComponent<LootController>();
        }

        public void InitStats(EnemyDefinition definition) 
            => _stats.InitStats(definition);

        public void SetTarget(Transform target) 
            => _ai.SetTarget(target);

        public void SetPositionAndRotation(Vector3 spawnPointPosition, Quaternion spawnPointRotation) 
            => _movement.SetPositionAndRotation(spawnPointPosition, spawnPointRotation);

        public void SetLoot(LootBagDefinition definitionLootBag) 
            => _loot.SetLoot(definitionLootBag);
    }
}