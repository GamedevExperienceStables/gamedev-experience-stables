using System.Collections.Generic;
using Game.Actors;
using UnityEngine;
using UnityEngine.AI;
using VContainer;
using VContainer.Unity;

namespace Game.Enemies
{
    public class EnemyFactory
    {
        private const float VALID_SPAWN_RADIUS = 2f;
        private readonly IObjectResolver _resolver;
        private readonly AbilityFactory _abilityFactory;


        [Inject]
        public EnemyFactory(IObjectResolver resolver, AbilityFactory abilityFactory)
        {
            _resolver = resolver;
            _abilityFactory = abilityFactory;
        }


        public EnemyController Create(EnemyDefinition definition, Transform spawnPoint, Transform target,
            Transform spawnContainer)
        {
            EnemyController enemy = Object.Instantiate(definition.Prefab, spawnContainer);
            _resolver.InjectGameObject(enemy.gameObject);

            enemy.InitStats(definition.InitialStats);
            AddAbilities(enemy, definition);
            enemy.SetAbilities(definition.InitialStats.AttackSettings);
            enemy.AddSpawn(spawnPoint);
            enemy.SetTarget(target);
            enemy.SetLoot(definition.LootBag);
            GetSpawnPoint(spawnPoint, enemy);
            enemy.InitSensor(definition.InitialStats);

            return enemy;
        }

        private static void GetSpawnPoint(Transform spawnPoint, EnemyController enemy)
        {
            Vector3 spawnPosition = spawnPoint.position;

            if (NavMesh.SamplePosition(spawnPosition, out NavMeshHit navmeshHit, VALID_SPAWN_RADIUS, NavMesh.AllAreas))
                spawnPosition = navmeshHit.position;

            enemy.SetPositionAndRotation(spawnPosition, spawnPoint.rotation);
        }

        private void AddAbilities(ActorController actor, EnemyDefinition definition)
        {
            RegisterAbilities(actor, definition.Abilities);
            actor.InitAbilities();
            GiveAbilities(actor, definition.Abilities);
        }

        private void RegisterAbilities(ActorController actor, List<AbilityDefinition> abilities)
        {
            foreach (AbilityDefinition definition in abilities)
            {
                ActorAbility ability = definition.CreateRuntimeInstance(_abilityFactory);
                actor.RegisterAbility(ability);
            }
        }

        private static void GiveAbilities(ActorController actor, List<AbilityDefinition> abilities)
        {
            foreach (AbilityDefinition definition in abilities)
                actor.GiveAbility(definition);
        }
    }
}