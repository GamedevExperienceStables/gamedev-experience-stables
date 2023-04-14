using Game.Actors;
using Game.Audio;
using Game.Cameras;
using Game.Dialog;
using Game.Enemies;
using Game.GameFlow;
using Game.Hero;
using Game.Level;
using Game.UI;
using Game.Utils;
using Game.Weapons;
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
        
        [SerializeField]
        private LocationAudioListener cameraAudioListener;

        [Header("UI")]
        [SerializeField]
        private GameplayView gameplayView;

        protected override void Configure(IContainerBuilder builder)
        {
            RegisterServices(builder);
            RegisterFactories(builder);

            RegisterUi(builder);
            RegisterCameras(builder);
            RegisterAudio(builder);
            RegisterPlanetStateMachine(builder);
            RegisterLocationStateMachine(builder);
            RegisterLootSystem(builder);
            RegisterAbilities(builder);
            RegisterProjectile(builder);
            RegisterEffects(builder);
            RegisterInteractions(builder);

            builder.RegisterEntryPoint<GameplayEntryPoint>();
        }

        private static void RegisterAbilities(IContainerBuilder builder)
        {
            builder.Register<AbilityFactory>(Lifetime.Singleton);

            builder.Register<AimAbility>(Lifetime.Transient);
            builder.Register<DashAbility>(Lifetime.Transient);
            builder.Register<RecoveryAbility>(Lifetime.Transient);
            builder.Register<ReviveAbility>(Lifetime.Transient);
            builder.Register<MeleeAbility>(Lifetime.Transient);
            builder.Register<AutoPickupAbility>(Lifetime.Transient);
            builder.Register<InteractionAbility>(Lifetime.Transient);
            builder.Register<ProjectileAbility>(Lifetime.Transient);
            builder.Register<ActiveSkillAbility>(Lifetime.Transient);
            builder.Register<StatsModifiersAbility>(Lifetime.Transient);
        }

        private static void RegisterEffects(IContainerBuilder builder)
        {
            builder.Register<EffectFactory>(Lifetime.Scoped);
            builder.Register<EffectHandler>(Lifetime.Scoped);
            
            builder.Register<EffectStatChange>(Lifetime.Transient);
            builder.Register<EffectStun>(Lifetime.Transient);
        }

        private static void RegisterProjectile(IContainerBuilder builder)
        {
            builder.Register<ProjectileFactory>(Lifetime.Scoped);
            builder.Register<ProjectileHandler>(Lifetime.Scoped);
        }

        private static void RegisterServices(IContainerBuilder builder)
        {
            builder.Register<GameplayPause>(Lifetime.Scoped);
            builder.Register<GameplayMenuInput>(Lifetime.Scoped);
            builder.Register<GameplayGameOver>(Lifetime.Scoped);
            builder.Register<GameplayInventory>(Lifetime.Scoped);
            builder.Register<LocationController>(Lifetime.Scoped);
            builder.Register<MagnetSystem>(Lifetime.Scoped).AsImplementedInterfaces();
            
            builder.Register<LocationMarkers>(Lifetime.Singleton);
            builder.Register<DialogService>(Lifetime.Singleton);

            builder.Register<GameplayPrefabFactory>(Lifetime.Scoped);
        }

        private static void RegisterInteractions(IContainerBuilder builder)
        {
            builder.Register<LocalizationInteraction>(Lifetime.Singleton);
            
            builder.Register<InteractionService>(Lifetime.Scoped);
            builder.Register<InteractionFactory>(Lifetime.Scoped);

            builder.Register<TransitionToLocationInteraction>(Lifetime.Transient);
            builder.Register<SaveGameInteraction>(Lifetime.Transient);
            builder.Register<LevelExitInteraction>(Lifetime.Transient);
            
            RegisterRocketContainer(builder);
        }

        private static void RegisterRocketContainer(IContainerBuilder builder)
        {
            builder.Register<RocketContainerInteraction>(Lifetime.Transient);
            builder.Register<RocketContainerHandler>(Lifetime.Transient);
        }

        private static void RegisterFactories(IContainerBuilder builder)
        {
            builder.Register<EnemyFactory>(Lifetime.Scoped);
            builder.Register<HeroFactory>(Lifetime.Scoped);
            builder.Register<TrapFactory>(Lifetime.Scoped);
        }

        private void RegisterUi(IContainerBuilder builder)
        {
            builder.Register<GameplayViewModel>(Lifetime.Scoped);
            builder.Register<InventoryViewModel>(Lifetime.Scoped);
            
            builder.Register<HudRuneSlotsView>(Lifetime.Scoped);
            builder.Register<HudRuneSlotsViewModel>(Lifetime.Scoped);
            
            builder.Register<InteractionView>(Lifetime.Scoped);
            builder.Register<InteractionViewModel>(Lifetime.Scoped);

            builder.Register<DialogView>(Lifetime.Scoped);
            builder.Register<DialogViewModel>(Lifetime.Scoped);
            
            builder.Register<SavingView>(Lifetime.Scoped);
            builder.Register<SavingViewModel>(Lifetime.Scoped);
            
            builder.Register<MiniMapView>(Lifetime.Scoped);
            builder.Register<MiniMapViewModel>(Lifetime.Scoped);

            builder.Register<PauseMenuViewModel>(Lifetime.Scoped);

            builder.RegisterInstance(gameplayView);
            builder.UseComponents(gameplayView.transform, componentsBuilder =>
            {
                componentsBuilder.AddInHierarchy<HudView>();
                componentsBuilder.AddInHierarchy<GameOverView>();
                componentsBuilder.AddInHierarchy<InventoryView>();
                
                componentsBuilder.AddInHierarchy<PauseView>();
                componentsBuilder.AddInHierarchy<PauseViewRouter>();
                
                componentsBuilder.AddInHierarchy<PauseMenuView>();
                componentsBuilder.AddInHierarchy<PauseSettingsView>();
                
                componentsBuilder.AddInHierarchy<ModalView>();
            });
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
            builder.Register<PlanetInventoryState>(Lifetime.Scoped);
            builder.Register<PlanetGameOverState>(Lifetime.Scoped);
            builder.Register<PlanetCompleteState>(Lifetime.Scoped);
            builder.Register<PlanetSaveGameState>(Lifetime.Scoped);
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

        private void RegisterAudio(IContainerBuilder builder)
            => builder.RegisterComponent(cameraAudioListener);
    }
}