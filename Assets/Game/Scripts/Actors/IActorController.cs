using Game.Stats;
using UnityEngine;

namespace Game.Actors
{
    public interface IActorController
    {
        bool HasStats<T>() where T : IStatsSet;
        T GetStats<T>() where T : IStatsSet;
        T FindAbility<T>() where T : ActorAbility;
        T GetComponent<T>();
        Transform Transform { get; }
    }
}