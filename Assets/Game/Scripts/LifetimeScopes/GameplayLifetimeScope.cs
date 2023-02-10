using Game.Actors;
using Game.Cameras;
using Game.Enemies;
using Game.GameFlow;
using Game.Hero;
using Game.Level;
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
            RegisterPlanetStateMachine(builder);
            RegisterLocationStateMachine(builder);
            RegisterLootSystem(builder);
            RegisterAbilities(builder);

            builder.RegisterEntryPoint<GameplayEntryPoint>();
        }
        
        private static void RegisterAbilities(IContainerBuilder builder)
        {
            builder.Register<AbilityFactory>(Lifetime.Singleton);

            builder.Register<AimAbility>(Lifetime.Transient);
            builder.Register<DashAbility>(Lifetime.Transient);
            builder.Register<RecoveryAbility>(Lifetime.Transient);
            builder.Register<MeleeAbility>(Lifetime.Transient);
            builder.Register<AutoPickupAbility>(Lifetime.Transient);
            builder.Register<InteractionAbility>(Lifetime.Transient);
            builder.Register<WeaponAbility>(Lifetime.Transient);
        }

        private static void RegisterServices(IContainerBuilder builder)
        {
            builder.Register<GameplayInputTracker>(Lifetime.Scoped);
            builder.Register<LocationController>(Lifetime.Scoped);
            builder.Register<MagnetSystem>(Lifetime.Scoped).AsImplementedInterfaces();

            RegisterInteractions(builder);
        }

        private static void RegisterInteractions(IContainerBuilder builder)
        {
            builder.Register<InteractionService>(Lifetime.Scoped);
            builder.Register<InteractionFactory>(Lifetime.Scoped);

            builder.Register<TransitionToLocationInteraction>(Lifetime.Transient);
            builder.Register<RocketContainerInteraction>(Lifetime.Transient);
            builder.Register<SaveGameInteraction>(Lifetime.Transient);
            builder.Register<LevelExitInteraction>(Lifetime.Transient);
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

        private static void RegisterPlanetStateMachine(IContainerBuilder builder)
        {
            builder.Register<PlanetStateMachine>(Lifetime.Scoped);

            builder.Register<PlanetLocationLoadingState>(Lifetime.Scoped);
            builder.Register<PlanetPlayState>(Lifetime.Scoped);
            builder.Register<PlanetPauseState>(Lifetime.Scoped);
            builder.Register<PlanetCompleteState>(Lifetime.Scoped);
        }

        private static void RegisterLocationStateMachine(IContainerBuilder builder)
        {
            builder.Register<LocationStateMachine>(Lifetime.Scoped);

            builder.Register<LocationSafeState>(Lifetime.Scoped);
            builder.Register<LocationBattleState>(Lifetime.Scoped);
        }

        private static void RegisterLootSystem(IContainerBuilder builder)
        {
            builder.Register<LootSpawner>(Lifetime.Scoped);
            builder.Register<LootFactory>(Lifetime.Scoped);
        }
    }
}