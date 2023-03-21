using System;
using System.Collections.Generic;
using Game.Actors;
using Game.Inventory;
using Game.Pet;
using Game.Player;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Game.Hero
{
    public sealed class HeroFactory : IDisposable
    {
        private readonly HeroDefinition _heroDefinition;
        private readonly PlayerController _player;
        private readonly IInventoryRunes _runes;
        private readonly AbilityFactory _abilityFactory;
        private readonly IObjectResolver _resolver;

        [Inject]
        public HeroFactory(
            HeroDefinition heroDefinition,
            PlayerController player,
            IInventoryRunes runes,
            AbilityFactory abilityFactory,
            IObjectResolver resolver
        )
        {
            _heroDefinition = heroDefinition;
            _player = player;
            _runes = runes;
            _abilityFactory = abilityFactory;
            _resolver = resolver;
        }

        public HeroController Create()
        {
            HeroController hero = _resolver.Instantiate(_heroDefinition.Prefab);
            _resolver.InjectGameObject(hero.gameObject);
            
            _player.BindHero(hero);
            AddAbilities(hero);
            GiveObtainedRunes(hero, _runes.Items);
            CreatePet(hero);

            return hero;
        }

        private void CreatePet(HeroController hero)
        {
            GameObject heroGameObject = hero.gameObject;
            
            Transform petPosition = heroGameObject.gameObject.GetComponent<HeroView>().PetPosition;
            GameObject pet = _resolver.Instantiate(_heroDefinition.Pet, 
                petPosition.position, 
                petPosition.rotation);
            pet.GetComponent<PetFollowing>().SetFollowingPosition(petPosition);
        }
        
        private void AddAbilities(ActorController actor)
        {
            RegisterAbilities(actor, _heroDefinition.Abilities);
            RegisterAbilities(actor, _heroDefinition.InitialAbilities);

            actor.InitAbilities();

            GiveAbilities(actor, _heroDefinition.InitialAbilities);
        }

        private void RegisterAbilities(ActorController actor, IEnumerable<AbilityDefinition> abilities)
        {
            foreach (AbilityDefinition definition in abilities)
            {
                ActorAbility ability = definition.CreateRuntimeInstance(_abilityFactory);
                actor.RegisterAbility(ability);
            }
        }

        private static void GiveAbilities(ActorController actor, IEnumerable<AbilityDefinition> abilities)
        {
            foreach (AbilityDefinition definition in abilities)
                actor.GiveAbility(definition);
        }

        private void GiveObtainedRunes(ActorController actor, IEnumerable<RuneDefinition> runesItems)
        {
            foreach (RuneDefinition runeDefinition in runesItems)
                actor.GiveAbility(runeDefinition.GrantAbility);
        }

        public void Dispose()
            => _player.UnbindHero();
    }
}