using VContainer;
using VContainer.Unity;

namespace Game.Hero
{
    public class HeroFactory
    {
        private readonly HeroDefinition _heroDefinition;
        private readonly PlayerData _playerState;
        private readonly IObjectResolver _resolver;

        [Inject]
        public HeroFactory(HeroDefinition heroDefinition, PlayerData playerState, IObjectResolver resolver)
        {
            _heroDefinition = heroDefinition;
            _playerState = playerState;
            _resolver = resolver;
        }

        public HeroController Create()
        {
            HeroController hero = _resolver.Instantiate(_heroDefinition.Prefab);
            _resolver.InjectGameObject(hero.gameObject);
            
            hero.Init(_playerState);

            return hero;
        }
    }
}