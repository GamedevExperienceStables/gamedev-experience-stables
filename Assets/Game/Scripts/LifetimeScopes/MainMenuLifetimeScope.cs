using Game.GameFlow;
using Game.UI;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Game.LifetimeScopes
{
    public class MainMenuLifetimeScope : LifetimeScope
    {
        [SerializeField]
        private MainMenuViewRouter mainMenuRouter;

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

            builder.RegisterComponent(mainMenuRouter);
            
            Transform uiRoot = mainMenuRouter.transform;
            builder.UseComponents(uiRoot, componentsBuilder =>
            {
                componentsBuilder.AddInHierarchy<StartMenuView>();
                componentsBuilder.AddInHierarchy<MainMenuSettingsView>();
                componentsBuilder.AddInHierarchy<AboutView>();
                componentsBuilder.AddInHierarchy<ArtView>();
            });
        }

        private static void RegisterStateMachine(IContainerBuilder builder)
        {
            builder.Register<MainMenuStateMachine>(Lifetime.Scoped);

            builder.Register<StartMenuState>(Lifetime.Scoped);
        }
    }
}