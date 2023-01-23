using Game.Stats;

namespace Game.Actors
{
    public interface IActorController
    {
        T GetStats<T>() where T : IStatsSet;
        T FindAbility<T>() where T : ActorAbilityView;
    }
}