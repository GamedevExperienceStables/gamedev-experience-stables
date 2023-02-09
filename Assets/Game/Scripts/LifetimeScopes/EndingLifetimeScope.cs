using Game.GameFlow.Ending;
using VContainer;
using VContainer.Unity;

namespace Game.LifetimeScopes
{
    public class EndingLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
            => builder.RegisterEntryPoint<EndingEntryPoint>();
    }
}