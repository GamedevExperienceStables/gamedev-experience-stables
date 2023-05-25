using Game.Audio;
using Game.CursorManagement;
using Game.GameFlow;
using Game.Hero;
using Game.Input;
using Game.Inventory;
using Game.Level;
using Game.Localization;
using Game.Persistence;
using Game.Player;
using Game.SceneManagement;
using Game.Settings;
using Game.TimeManagement;
using Game.UI;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Game.LifetimeScopes
{
    public class RootLifetimeScope : LifetimeScope
    {
        [SerializeField]
        private GameSettings gameSettings;

        [SerializeField]
        private UiSettings uiSettings;

        [SerializeField]
        private FxSettings fxSettings;

        [SerializeField]
        private HeroDefinition heroData;

        [SerializeField]
        private GameDataTables dataTables;

        protected override void Configure(IContainerBuilder builder)
        {
            base.Configure(builder);

            RegisterSceneLoader(builder);
            RegisterInput(builder);
            RegisterStateMachine(builder);
            RegisterSettings(builder);
            RegisterData(builder);
            RegisterDataTables(builder);
            RegisterServices(builder);
            RegisterSaveSystem(builder);
            RegisterAudio(builder);
            RegisterTime(builder);
            RegisterUi(builder);

            builder.RegisterEntryPoint<GameEntryPoint>();
        }

        private static void RegisterData(IContainerBuilder builder)
        {
            builder.Register<GameData>(Lifetime.Singleton);
            builder.Register<GameImportExport>(Lifetime.Singleton);

            builder.Register<LevelData>(Lifetime.Singleton);
            builder.Register<LevelController>(Lifetime.Singleton);
            builder.Register<LevelImportExport>(Lifetime.Singleton);
            builder.Register<LevelGoalChecker>(Lifetime.Singleton);

            builder.Register<PlayerData>(Lifetime.Singleton);
            builder.Register<PlayerController>(Lifetime.Singleton);
            builder.Register<PlayerImportExport>(Lifetime.Singleton);
            builder.Register<InventoryData>(Lifetime.Singleton);
            builder.Register<InventoryController>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();

            builder.Register<LocationData>(Lifetime.Singleton);
            
            builder.Register<PlayerGamePrefs>(Lifetime.Singleton);
            builder.Register<PlayerAudioPrefs>(Lifetime.Singleton);
            builder.Register<PlayerGraphicsPrefs>(Lifetime.Singleton);
            builder.Register<PlayerLocalizationPrefs>(Lifetime.Singleton);
        }

        private static void RegisterServices(IContainerBuilder builder)
        {
            builder.Register<QuitGameService>(Lifetime.Singleton);
            builder.Register<UnityLocalization>(Lifetime.Singleton).As<ILocalizationService>();
            builder.Register<CursorService>(Lifetime.Singleton);

            builder.Register<ModalService>(Lifetime.Singleton);
            
            builder.Register<UiFxService>(Lifetime.Scoped);
        }

        private static void RegisterInput(IContainerBuilder builder)
        {
            builder.Register<GameInputControlsAdapter>(Lifetime.Singleton);

            builder.Register<InputService>(Lifetime.Singleton).As<IInputService>();
            builder.Register<InputBindings>(Lifetime.Singleton);
            builder.Register<InputControlGameplay>(Lifetime.Singleton).As<IInputControlGameplay>();
            builder.Register<InputControlMenu>(Lifetime.Singleton).As<IInputControlMenu>();
        }

        private void RegisterSceneLoader(IContainerBuilder builder)
        {
            builder.Register<SceneLoader>(Lifetime.Singleton);

            builder.RegisterComponentInNewPrefab(gameSettings.FaderSettings.FadeScreen, Lifetime.Singleton)
                .DontDestroyOnLoad()
                .As<IFaderScreen>();

            builder.RegisterComponentInNewPrefab(gameSettings.FaderSettings.LoadingScreen, Lifetime.Singleton)
                .DontDestroyOnLoad()
                .As<ILoadingScreen>();
        }

        private static void RegisterStateMachine(IContainerBuilder builder)
        {
            builder.Register<RootStateMachine>(Lifetime.Singleton);

            builder.Register<InitState>(Lifetime.Singleton);
            builder.Register<IntroState>(Lifetime.Singleton);
            builder.Register<MainMenuState>(Lifetime.Singleton);
            builder.Register<NewGameState>(Lifetime.Singleton);
            builder.Register<LoadGameState>(Lifetime.Singleton);
            builder.Register<CompleteLevelState>(Lifetime.Singleton);
            builder.Register<CompleteGameState>(Lifetime.Singleton);
            builder.Register<CreditsGameState>(Lifetime.Singleton);
            builder.Register<QuitGameState>(Lifetime.Singleton);
            builder.Register<PlanetState>(Lifetime.Singleton);
        }

        private void RegisterSettings(IContainerBuilder builder)
        {
            builder.RegisterInstance(heroData);
            builder.RegisterInstance(heroData.InitialStats);

            builder.RegisterInstance(gameSettings.CameraSettings);
            builder.RegisterInstance(gameSettings.LootSettings);
            builder.RegisterInstance(gameSettings.LevelsSettings);
            builder.RegisterInstance(gameSettings.SaveSettings);
            builder.RegisterInstance(gameSettings.MagnetSettings);
            builder.RegisterInstance(gameSettings.InventorySettings);
            builder.RegisterInstance(gameSettings.AudioSettings);
            builder.RegisterInstance(gameSettings.CursorSettings);
            builder.RegisterInstance(gameSettings.PoolSettings);
            
            builder.RegisterInstance(uiSettings.StartMenu);
            builder.RegisterInstance(uiSettings.SettingsMenu);
            builder.RegisterInstance(uiSettings.SettingsControlsMenuMenu);
            builder.RegisterInstance(uiSettings.PauseMenu);
            builder.RegisterInstance(uiSettings.PauseSettings);
            builder.RegisterInstance(uiSettings.PauseHelp);
            builder.RegisterInstance(uiSettings.GameOver);
            builder.RegisterInstance(uiSettings.Interaction);
            builder.RegisterInstance(uiSettings.Hud);
            builder.RegisterInstance(uiSettings.Loading);
            builder.RegisterInstance(uiSettings.Saving);
            builder.RegisterInstance(uiSettings.Notification);
            builder.RegisterInstance(uiSettings.InputBindings);

            builder.RegisterInstance(fxSettings.Inventory);
            builder.RegisterInstance(fxSettings.HudRunes);
            builder.RegisterInstance(fxSettings.HudDamage);
            builder.RegisterInstance(fxSettings.GameOver);
            builder.RegisterInstance(fxSettings.Common);
        }

        private static void RegisterUi(IContainerBuilder builder)
        {
            builder.Register<SettingsView>(Lifetime.Scoped);
            builder.Register<SettingsViewModel>(Lifetime.Singleton);
            
            builder.Register<ModalViewModel>(Lifetime.Singleton);

            builder.Register<Typewriter>(Lifetime.Transient);
        }

        private void RegisterDataTables(IContainerBuilder builder)
        {
            builder.RegisterInstance(dataTables.Runes);
            builder.RegisterInstance(dataTables.Materials);
            builder.RegisterInstance(dataTables.Locations);
            builder.RegisterInstance(dataTables.Loot);
        }

        private void RegisterSaveSystem(IContainerBuilder builder)
        {
            builder.Register<PersistenceService>(Lifetime.Singleton);

            builder.Register<PrefsPersistence>(Lifetime.Singleton).As<IPlayerPrefs>();
            builder.Register<LocalPersistence>(Lifetime.Singleton).As<IPersistence>();
            builder.Register<NewtonJsonDataSerializer>(Lifetime.Singleton).As<IDataSerializer>()
                .WithParameter(gameSettings.SaveSettings.Formatting);
        }
        
        private static void RegisterAudio(IContainerBuilder builder)
        {
            builder.Register<FmodService>(Lifetime.Singleton).As<IAudioService>().As<IAudioTuner>().AsSelf();
            builder.Register<FmodFootsteps>(Lifetime.Singleton).As<IFootstepsAudio>();
            builder.Register<FootstepsEmitter>(Lifetime.Transient);
        }
        
        private static void RegisterTime(IContainerBuilder builder)
        {
            builder.Register<TimeService>(Lifetime.Singleton).AsImplementedInterfaces();
            
            builder.Register<TimerFactory>(Lifetime.Singleton);
            builder.Register<TimerPool>(Lifetime.Singleton);
            builder.Register<TimerUpdater>(Lifetime.Singleton).AsImplementedInterfaces();
        }
    }
}