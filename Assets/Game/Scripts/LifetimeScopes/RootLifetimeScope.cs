using Game.GameFlow;
using Game.Hero;
using Game.Input;
using Game.Level;
using Game.Persistence;
using Game.RandomManagement;
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
        private HeroDefinition heroData;

        protected override void Configure(IContainerBuilder builder)
        {
            base.Configure(builder);

            RegisterSceneLoader(builder);
            RegisterInput(builder);
            RegisterStateMachine(builder);
            RegisterSettings(builder);
            RegisterData(builder);
            RegisterServices(builder);
            RegisterSaveSystem(builder);
        }

        private static void RegisterData(IContainerBuilder builder)
        {
            builder.Register<GameData>(Lifetime.Singleton);
            builder.Register<GameDataHandler>(Lifetime.Singleton);

            builder.Register<LevelData>(Lifetime.Singleton);
            builder.Register<LevelDataHandler>(Lifetime.Singleton);
            builder.Register<LevelImportExport>(Lifetime.Singleton);

            builder.Register<PlayerData>(Lifetime.Singleton);
            builder.Register<PlayerDataHandler>(Lifetime.Singleton);

            builder.Register<LocationData>(Lifetime.Singleton);
            builder.Register<LocationDataHandler>(Lifetime.Singleton);
        }

        private static void RegisterServices(IContainerBuilder builder)
        {
            builder.Register<TimeService>(Lifetime.Singleton);
            builder.Register<RandomService>(Lifetime.Singleton);
            builder.Register<QuitGameService>(Lifetime.Singleton);
        }

        private static void RegisterInput(IContainerBuilder builder)
        {
            builder.Register<GameInputControlsAdapter>(Lifetime.Singleton);

            builder.Register<InputService>(Lifetime.Singleton).As<IInputService>();
            builder.Register<InputControlGameplay>(Lifetime.Singleton).As<IInputControlGameplay>();
            builder.Register<InputControlMenu>(Lifetime.Singleton).As<IInputControlMenu>();
        }

        private void RegisterSceneLoader(IContainerBuilder builder)
        {
            builder.Register<SceneLoader>(Lifetime.Singleton);

            builder.RegisterComponentInNewPrefab(gameSettings.UiSettings.FadeScreen, Lifetime.Singleton)
                .DontDestroyOnLoad()
                .As<IFaderScreen>();

            builder.RegisterComponentInNewPrefab(gameSettings.UiSettings.LoadingScreen, Lifetime.Singleton)
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
        }

        private void RegisterSaveSystem(IContainerBuilder builder)
        {
            builder.Register<PersistenceService>(Lifetime.Singleton);

            builder.Register<LocalPersistence>(Lifetime.Singleton).As<IPersistence>();
            builder.Register<NewtonJsonDataSerializer>(Lifetime.Singleton).As<IDataSerializer>()
                .WithParameter(gameSettings.SaveSettings.Formatting);
        }
    }
}