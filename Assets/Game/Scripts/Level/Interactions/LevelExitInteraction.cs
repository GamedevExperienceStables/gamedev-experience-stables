using Game.GameFlow;

namespace Game.Level
{
    public class LevelExitInteraction : Interaction
    {
        private readonly LevelGoalChecker _level;
        private readonly PlanetStateMachine _planetStateMachine;

        public LevelExitInteraction(LevelGoalChecker level, PlanetStateMachine planetStateMachine)
        {
            _level = level;
            _planetStateMachine = planetStateMachine;
        }

        public override bool CanExecute() 
            => _level.AreGoalsMet();

        public override void Execute()
        {
            _planetStateMachine.EnterState<PlanetCompleteState>();
        }
    }
}