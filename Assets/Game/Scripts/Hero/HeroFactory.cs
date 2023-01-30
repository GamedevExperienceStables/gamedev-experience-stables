using System.Collections.Generic;
using Game.Actors;
using Game.Player;
using VContainer;
using VContainer.Unity;

namespace Game.Hero
{
    public class HeroFactory
    {
        private readonly HeroDefinition _heroDefinition;
        private readonly PlayerController _player;
        private readonly AbilityFactory _abilityFactory;
        private readonly IObjectResolver _resolver;

        [Inject]
        public HeroFactory(HeroDefinition heroDefinition, PlayerController player, AbilityFactory abilityFactory,
            IObjectResolver resolver)
        {
            _heroDefinition = heroDefinition;
            _player = player;
            _abilityFactory = abilityFactory;
            _resolver = resolver;
        }

        public HeroController Create()
        {
            HeroController hero = _resolver.Instantiate(_heroDefinition.Prefab);
            _resolver.InjectGameObject(hero.gameObject);
            
            hero.Bind(_player.GetStats());
            AddAbilities(hero);

            return hero;
        }

        private void AddAbilities(ActorController actor)
        {
            RegisterAbilities(actor, _heroDefinition.Abilities);
            RegisterAbilities(actor, _heroDefinition.InitialAbilities);

            actor.InitAbilities();

            GiveAbilities(actor, _heroDefinition.InitialAbilities);
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