using System;
using Game.GameFlow;
using Game.Hero;
using Game.Stats;
using VContainer;

namespace Game.UI
{
    public class GameplayViewModel
    {
        private RootStateMachine _rootStateMachine;
        private PlanetStateMachine _planetStateMachine;

        public IReadOnlyCharacterStatWithMax HeroHealth { get; private set; }
        public IReadOnlyCharacterStatWithMax HeroMana { get; private set; }
        public IReadOnlyCharacterStatWithMax HeroStamina { get; private set; }

        [Inject]
        public void Construct(
            RootStateMachine rootStateMachine,
            PlanetStateMachine planetStateMachine,
            PlayerData playerData
        )
        {
            _planetStateMachine = planetStateMachine;
            _rootStateMachine = rootStateMachine;

            HeroHealth = playerData.Stats.Health;
            HeroMana = playerData.Stats.Mana;
            HeroStamina = playerData.Stats.Stamina;
        }

        public void PauseGame() => _planetStateMachine.PushState<PlanetPauseState>();
        public void ResumeGame() => _planetStateMachine.PopState();

        public void GoToMainMenu() => _rootStateMachine.EnterState<MainMenuState>();
    }
}