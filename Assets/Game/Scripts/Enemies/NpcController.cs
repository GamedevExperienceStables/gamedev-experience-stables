using Game.Actors;
using Game.Stats;
using UnityEngine;

namespace Game.Enemies
{
    public class NpcController : ActorController
    {
        public override IStatsSet Stats { get; }
        
        public virtual void SetPositionAndRotation(Vector3 spawnPointPosition, Quaternion spawnPointRotation)
        {
        }
    }
}