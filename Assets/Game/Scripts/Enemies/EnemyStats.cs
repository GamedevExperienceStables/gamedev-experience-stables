using System;
using Game.Actors;
using UnityEngine;

namespace Game.Enemies
{
    public class EnemyStats : ActorStats
    {
        public EnemyStats()
        {
            this.CreateStatsHealth();
            this.CreateStatsMovement();
        }

        public void InitStats(InitialStats stats)
        {
            this.InitStatsHealth(stats.HealthPoints);
            this.InitStatsMovement(stats.MovementSpeed);
        }
        
        [Serializable]
        public class InitialStats
        {
            [SerializeField]
            private float healthPoints = 1f;
            
            [Header("Movement")]
            [SerializeField]
            private float movementSpeed = 3f;

            public float HealthPoints => healthPoints;

            public float MovementSpeed => movementSpeed;
        }
    }
}