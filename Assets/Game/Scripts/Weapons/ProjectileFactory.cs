using VContainer;
using VContainer.Unity;

namespace Game.Weapons
{
    public class ProjectileFactory
    {
        private readonly IObjectResolver _resolver;

        [Inject]
        public ProjectileFactory(IObjectResolver resolver)
            => _resolver = resolver;

        public Projectile Create(ProjectileDefinition definition)
        {
            Projectile projectile = _resolver.Instantiate(definition.Prefab);
            projectile.Init(definition);
            projectile.gameObject.SetActive(false);

            return projectile;
        }
    }
}