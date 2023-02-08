using System.Collections.Generic;
using Game.Actors;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Game.Enemies
{
    public class EnemyFactory
    {
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
            enemy.SetTarget(target);
            enemy.SetLoot(definition.LootBag);
            enemy.SetPositionAndRotation(spawnPoint.position, spawnPoint.rotation);

            return enemy;
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