using Game.Achievements;
using Game.Actors;
using Game.Audio;
using Game.Cameras;
using Game.Dialog;
using Game.Enemies;
using Game.GameFlow;
using Game.Hero;
using Game.Level;
using Game.UI;
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
            RegisterUiFx(builder);
            RegisterCameras(builder);
            RegisterHero(builder);
            RegisterAudio(builder);
            RegisterPlanetStateMachine(builder);
            RegisterLootSystem(builder);
            RegisterAbilities(builder);
            RegisterProjectile(builder);
            RegisterEffects(builder);
            RegisterInteractions(builder);
            RegisterAchievements(builder);

            builder.RegisterEntryPoint<GameplayEntryPoint>();
        }

        private static void RegisterAchievements(IContainerBuilder builder) 
            => builder.Register<InventoryAchievementsHandler>(Lifetime.Singleton).AsImplementedInterfaces();

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

        private static void RegisterHero(IContainerBuilder builder) 
            => builder.Register<HeroStaffColorChanger>(Lifetime.Singleton).AsImplementedInterfaces();

        private static void RegisterEffects(IContainerBuilder builder)
        {
            builder.Register<EffectFactory>(Lifetime.Scoped);
            builder.Register<EffectHandler>(Lifetime.Scoped);
            
            builder.Register<EffectStatChange>(Lifetime.Transient);
            builder.Register<EffectStun>(Lifetime.Transient);
        }

        private static void RegisterProjectile(IContainerBuilder builder)
        {
            builder.Register<ProjectilePool>(Lifetime.Singleton);
            builder.Register<ProjectileFactory>(Lifetime.Singleton);
            builder.Register<ProjectileHandler>(Lifetime.Singleton);
            builder.Register<TargetingHandler>(Lifetime.Singleton);
        }

        private static void RegisterServices(IContainerBuilder builder)
        {
            builder.Register<GameplayPause>(Lifetime.Singleton);
            builder.Register<GameplayMenuInput>(Lifetime.Singleton);
            builder.Register<GameplayInventory>(Lifetime.Singleton);
            
            builder.Register<LocationController>(Lifetime.Singleton);
            builder.Register<LocationContextHandler>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
            builder.Register<LocationStateStore>(Lifetime.Singleton);
            builder.Register<LocationStateStoreLoot>(Lifetime.Singleton);
            builder.Register<LocationStateStoreCounter>(Lifetime.Singleton);
            builder.Register<LocationStateStoreFact>(Lifetime.Singleton);
            
            builder.Register<MagnetSystem>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<GameplayGameOver>(Lifetime.Singleton).AsImplementedInterfaces();
            
            builder.Register<LocationMarkers>(Lifetime.Singleton);
            builder.Register<DialogService>(Lifetime.Singleton);
            builder.Register<DialogNotification>(Lifetime.Singleton).AsImplementedInterfaces();

            builder.Register<PrefabFactory>(Lifetime.Singleton);
            builder.Register<SpawnPool>(Lifetime.Singleton);
        }

        private static void RegisterInteractions(IContainerBuilder builder)
        {
            builder.Register<LocalizationInteraction>(Lifetime.Singleton);
            builder.Register<RocketContainerLocalization>(Lifetime.Singleton);
            
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
            builder.Register<HudPromptView>(Lifetime.Singleton);
            builder.Register<HudDamageView>(Lifetime.Singleton);

            builder.Register<GameplayViewModel>(Lifetime.Singleton);
            builder.Register<InventoryViewModel>(Lifetime.Singleton);
            
            builder.Register<HudRuneSlotsView>(Lifetime.Singleton);
            builder.Register<HudRuneSlotsViewModel>(Lifetime.Singleton);
            
            builder.Register<InteractionView>(Lifetime.Singleton);
            builder.Register<InteractionViewModel>(Lifetime.Singleton);

            builder.Register<DialogView>(Lifetime.Singleton);
            builder.Register<DialogViewModel>(Lifetime.Singleton);
            
            builder.Register<SavingView>(Lifetime.Singleton);
            builder.Register<SavingViewModel>(Lifetime.Singleton);
            
            builder.Register<MiniMapView>(Lifetime.Singleton);
            builder.Register<MiniMapViewModel>(Lifetime.Singleton);

            builder.Register<PauseMenuViewModel>(Lifetime.Singleton);
            
            builder.Register<HelpContentView>(Lifetime.Singleton);
            builder.Register<ControlTemplateViewFactory>(Lifetime.Singleton);
            builder.Register<ControlTemplateView>(Lifetime.Transient);
            

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
                componentsBuilder.AddInHierarchy<PauseHelpView>();
                
                componentsBuilder.AddInHierarchy<ModalView>();
            });
        }
        
        private static void RegisterUiFx(IContainerBuilder builder)
        {
            builder.Register<InventoryFx>(Lifetime.Singleton);
            builder.Register<HudRunesFx>(Lifetime.Singleton);
            builder.Register<HudDamageFx>(Lifetime.Singleton);
            builder.Register<GameOverFx>(Lifetime.Singleton);
            builder.Register<CommonFx>(Lifetime.Singleton);
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

        private static void RegisterLootSystem(IContainerBuilder builder)
        {
            builder.Register<LootSpawner>(Lifetime.Scoped);
            builder.Register<LootFactory>(Lifetime.Scoped);
        }

        private void RegisterAudio(IContainerBuilder builder)
            => builder.RegisterComponent(cameraAudioListener);
    }
}