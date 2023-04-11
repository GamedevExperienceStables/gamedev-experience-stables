using Game.UI;
using VContainer;
using VContainer.Unity;

namespace Game.GameFlow
{
    public class IntroLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
            => builder.Register<CutsceneView>(Lifetime.Singleton);
    }
}