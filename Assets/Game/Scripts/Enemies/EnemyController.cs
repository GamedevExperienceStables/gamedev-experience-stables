using Game.Actors;
using Game.Level;
using Game.Stats;
using UnityEngine;

namespace Game.Enemies
{
    [RequireComponent(typeof(NavigationController))]
    public class EnemyController : ActorController
    {
        private AiController _ai;
        private MovementController _movement;
        private LootController _loot;
        
        private EnemyStats _stats;
        protected override IStats Stats => _stats;

        protected override void OnActorAwake()
        {
            _ai = GetComponent<AiController>();
            _movement = GetComponent<MovementController>();
            _loot = GetComponent<LootController>();

            _stats = new();
        }

        protected override void OnActorDestroy() 
            => _stats.Dispose();

        public void InitStats(EnemyStats.InitialStats initial) 
            => _stats.InitStats(initial);

        public void SetTarget(Transform target) 
            => _ai.SetTarget(target);

        public void SetPositionAndRotation(Vector3 spawnPointPosition, Quaternion spawnPointRotation) 
            => _movement.SetPositionAndRotation(spawnPointPosition, spawnPointRotation);

        public void SetLoot(LootBagDefinition definitionLootBag) 
            => _loot.SetLoot(definitionLootBag);
    }
}