using Game.GameFlow;
using Game.Settings;
using Game.UI;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Game.LifetimeScopes
{
    public class MainMenuLifetimeScope : LifetimeScope
    {
        [SerializeField]
        private UiSettings uiSettings;

        protected override void Configure(IContainerBuilder builder)
        {
            RegisterStateMachine(builder);

            RegisterMainMenu(builder);
            RegisterAbout(builder);
        }

        private static void RegisterMainMenu(IContainerBuilder builder)
        {
            builder.Register<StartMenuViewModel>(Lifetime.Scoped);
            builder.Register<AboutViewModel>(Lifetime.Scoped);
            builder.Register<ArtViewModel>(Lifetime.Scoped);

            builder.UseComponents(componentsBuilder =>
            {
                componentsBuilder.AddInHierarchy<MainMenuViewRouter>();
                componentsBuilder.AddInHierarchy<StartMenuView>();
                componentsBuilder.AddInHierarchy<MainMenuSettingsView>();
                componentsBuilder.AddInHierarchy<AboutView>();
                componentsBuilder.AddInHierarchy<ArtView>();
                componentsBuilder.AddInHierarchy<ModalView>();
            });
        }

        private void RegisterAbout(IContainerBuilder builder)
        {
            builder.RegisterInstance(uiSettings.About);

            builder.Register<TeamsView>(Lifetime.Scoped);

            builder.Register<TeamViewFactory>(Lifetime.Scoped);
            builder.Register<TeamView>(Lifetime.Transient);

            builder.Register<EmployeeViewFactory>(Lifetime.Scoped);
            builder.Register<EmployeeView>(Lifetime.Transient);
        }

        private static void RegisterStateMachine(IContainerBuilder builder)
        {
            builder.Register<MainMenuStateMachine>(Lifetime.Scoped);

            builder.Register<StartMenuState>(Lifetime.Scoped);
        }
    }
}