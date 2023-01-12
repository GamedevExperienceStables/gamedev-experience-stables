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
        private StartMenuView startMenuView;

        protected override void Configure(IContainerBuilder builder)
        {
            RegisterStateMachine(builder);

            RegisterStartMenu(builder);
        }

        private void RegisterStartMenu(IContainerBuilder builder)
        {
            builder.RegisterComponent(startMenuView);
            builder.Register<StartMenuViewModel>(Lifetime.Scoped);
        }

        private static void RegisterStateMachine(IContainerBuilder builder)
        {
            builder.Register<MainMenuStateMachine>(Lifetime.Scoped);
            
            builder.Register<StartMenuState>(Lifetime.Scoped);
        }
    }
}