using Game.Settings;
using Game.UI;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Game.LifetimeScopes
{
    public class CutsceneLifetimeScope : LifetimeScope
    {
        [SerializeField]
        private UiSettings settings;
        
        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<Cutscene>(Lifetime.Singleton);
            builder.Register<CutsceneActionNext>(Lifetime.Transient);
            builder.Register<CutsceneActionSkip>(Lifetime.Transient);

            builder.RegisterInstance(settings.Cutscene);
        }
    }
}