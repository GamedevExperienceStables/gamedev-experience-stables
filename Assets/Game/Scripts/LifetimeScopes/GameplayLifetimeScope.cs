using Game.Cameras;
using Game.Enemies;
using Game.GameFlow;
using Game.Hero;
using Game.Level;
using Game.TimeManagement;
using Game.UI;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Game.LifetimeScopes
{
    public class GameplayLifetimeScope : LifetimeScope
    {
        [SerializeField]
        private SceneCamera sceneCamera;

        [SerializeField]
        private FollowSceneCamera followCamera;

        [Header("UI")]
        [SerializeField]
        private GameplayView gameplayView;

        [SerializeField]
        private HudView hudView;

        [SerializeField]
        private PauseMenuView pauseMenuView;

        protected override void Configure(IContainerBuilder builder)
        {
            RegisterServices(builder);
            RegisterFactories(builder);

            RegisterUi(builder);
            RegisterCameras(builder);
            RegisterStateMachine(builder);

            builder.UseEntryPoints(entryPoints =>
            {
                entryPoints.Add<GameplayEntryPoint>();
                entryPoints.Add<InGameInputListener>();
            });
        }

        private static void RegisterServices(IContainerBuilder builder)
        {
            builder.Register<LocationController>(Lifetime.Scoped);
            builder.Register<TimeService>(Lifetime.Scoped);

            RegisterInteractions(builder);
        }

        private static void RegisterInteractions(IContainerBuilder builder)
        {
            builder.Register<InteractionService>(Lifetime.Scoped);
            builder.Register<InteractionFactory>(Lifetime.Scoped);

            builder.Register<TransitionToLocationInteraction>(Lifetime.Transient);
        }

        private static void RegisterFactories(IContainerBuilder builder)
        {
            builder.Register<EnemyFactory>(Lifetime.Scoped);
            builder.Register<HeroFactory>(Lifetime.Scoped);
        }

        private void RegisterUi(IContainerBuilder builder)
        {
            builder.Register<GameplayViewModel>(Lifetime.Scoped);

            builder.RegisterComponent(gameplayView);
            builder.RegisterComponent(hudView);
            builder.RegisterComponent(pauseMenuView);
        }

        private void RegisterCameras(IContainerBuilder builder)
        {
            builder.RegisterComponent(sceneCamera);
            builder.RegisterComponent(followCamera);
        }

        private static void RegisterStateMachine(IContainerBuilder builder)
        {
            builder.Register<PlanetStateMachine>(Lifetime.Scoped);

            builder.Register<PlanetLocationLoadingState>(Lifetime.Scoped);
            builder.Register<PlanetPlayState>(Lifetime.Scoped);
            builder.Register<PlanetPauseState>(Lifetime.Scoped);
        }
    }
}