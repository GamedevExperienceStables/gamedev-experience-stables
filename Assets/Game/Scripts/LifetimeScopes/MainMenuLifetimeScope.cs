using Game.GameFlow;
using Game.UI;
using VContainer;
using VContainer.Unity;

namespace Game.LifetimeScopes
{
    public class MainMenuLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            RegisterStateMachine(builder);

            RegisterMainMenu(builder);
        }

        private void RegisterMainMenu(IContainerBuilder builder)
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

        private static void RegisterStateMachine(IContainerBuilder builder)
        {
            builder.Register<MainMenuStateMachine>(Lifetime.Scoped);

            builder.Register<StartMenuState>(Lifetime.Scoped);
        }
    }
}