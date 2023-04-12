using Game.UI;
using VContainer;
using VContainer.Unity;

namespace Game.LifetimeScopes
{
    public class EndingLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
            => builder.Register<Cutscene>(Lifetime.Singleton);
    }
}