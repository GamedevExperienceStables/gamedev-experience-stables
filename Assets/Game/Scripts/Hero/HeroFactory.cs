using System;
using System.Collections.Generic;
using Game.Actors;
using Game.Inventory;
using Game.Pet;
using Game.Player;
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
            HeroController hero = CreateHero();
            PetController pet = CreatePet();
            hero.BindPet(pet);
            
            _player.BindHero(hero);
            
            AddAbilities(hero);
            GiveObtainedRunes(hero, _runes.Items);

            return hero;
        }

        private HeroController CreateHero()
        {
            HeroController hero = _resolver.Instantiate(_heroDefinition.Prefab);
            _resolver.InjectGameObject(hero.gameObject);
            return hero;
        }

        private PetController CreatePet() 
            => _resolver.Instantiate(_heroDefinition.PetPrefab);

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