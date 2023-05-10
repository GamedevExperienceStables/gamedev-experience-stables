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

        public Projectile Create(Projectile prefab, IProjectileSettings settings)
        {
            Projectile projectile = _resolver.Instantiate(prefab);
            projectile.gameObject.SetActive(false);

            int originId = prefab.GetInstanceID();
            projectile.Init(settings, originId);

            return projectile;
        }
    }
}