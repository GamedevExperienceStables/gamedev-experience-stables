using Game.Settings;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Game.LifetimeScopes
{
    public class CreditsLifetimeScope : LifetimeScope
    {
        [SerializeField]
        private UiSettings settings;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterInstance(settings.InputBindings);
            builder.RegisterInstance(settings.Cutscene);
            builder.RegisterInstance(settings.Credits);
        }
    }
}