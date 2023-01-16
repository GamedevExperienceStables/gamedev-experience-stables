using Game.GameFlow;
using Game.Hero;
using Game.Input;
using Game.SceneManagement;
using Game.Settings;
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

        [SerializeField]
        private FaderScreenView faderScreenPrefab;

        [SerializeField]
        private LoadingScreenView loadingScreenPrefab;

        protected override void Configure(IContainerBuilder builder)
        {
            base.Configure(builder);

            RegisterSceneLoader(builder);
            RegisterInput(builder);
            RegisterStateMachine(builder);
            RegisterSettings(builder);

            builder.Register<GameStateModel>(Lifetime.Singleton);
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

            builder.RegisterComponentInNewPrefab(faderScreenPrefab, Lifetime.Singleton)
                .DontDestroyOnLoad()
                .As<IFaderScreen>();

            builder.RegisterComponentInNewPrefab(loadingScreenPrefab, Lifetime.Singleton)
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
            builder.RegisterInstance(gameSettings.CameraSettings);
            builder.RegisterInstance(gameSettings.InitialSettings);
        }
    }
}