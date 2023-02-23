using VContainer;
using VContainer.Unity;

namespace Game.Weapons
{
    public class ProjectileFactory
    {
        private readonly IObjectResolver _resolver;

        public ProjectileFactory(IObjectResolver resolver) 
            => _resolver = resolver;

        public Projectile Create(ProjectileDefinition definition)
        {
            Projectile instance = _resolver.Instantiate(definition.Prefab);
            instance.Init(
                definition.CollisionLayers, 
                definition.Speed,
                definition.LifeTime, 
                definition.Damages
            );

            return instance;
        }
    }
}