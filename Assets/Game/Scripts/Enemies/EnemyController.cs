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
        private AiSensor _sensor;
        private MovementController _movement;
        private LootController _loot;
        private MeleeAbility _melee;
        private WeaponAbility _weapon;
        private EnemyStats _stats;
        private IActorController _owner;
        
        public Transform SpawnPoint { get; private set; }
        
        protected override IStats Stats => _stats;

        protected override void OnActorAwake()
        {
            _owner = GetComponent<IActorController>();
            _ai = GetComponent<AiController>();
            _movement = GetComponent<MovementController>();
            _loot = GetComponent<LootController>();
            _sensor = GetComponent<AiSensor>();
        }

        protected override void OnActorDestroy() 
            => _stats.Dispose();

        public void InitStats(EnemyStats.InitialStats initial)
        {
            _stats = new();
            _stats.InitStats(initial);
        }    

        public void AddSpawn(Transform spawnPoint) 
            => SpawnPoint = spawnPoint;
        
        public void SetTarget(Transform target) 
            => _ai.SetTarget(target);

        public void SetPositionAndRotation(Vector3 spawnPointPosition, Quaternion spawnPointRotation) 
            => _movement.SetPositionAndRotation(spawnPointPosition, spawnPointRotation);

        public void SetLoot(LootBagDefinition definitionLootBag) 
            => _loot.SetLoot(definitionLootBag);
        
        public void InitSensor(EnemyStats.InitialStats initial)
            => _sensor.InitSensor(initial);

        public void SetAbilities()
        {
            _melee = _owner.GetAbility<MeleeAbility>();
            _weapon = _owner.GetAbility<WeaponAbility>();

        }
        
        public void MeleeAttack()
            => _melee.TryActivateAbility();
        
        public void RangeAttack()
            => _weapon.TryActivateAbility();
    }
}