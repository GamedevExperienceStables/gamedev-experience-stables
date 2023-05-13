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
            this.CreateStatsWeight();
            this.CreateStatsStamina();
        }

        public void InitStats(InitialStats stats)
        {
            this.InitStatsHealth(stats.HealthPoints);
            this.InitStatsMovement(stats.MovementSpeed);
            this.InitStatsWeight(stats.Weight);
            this.InitStatsStamina(stats.StaminaPoints);
        }
        
        [Serializable]
        public class InitialStats
        {
            [SerializeField, Min(1)]
            private float healthPoints = 1f;
            
            [SerializeField, Min(0)]
            private float staminaPoints = 1f;
            
            [Header("Movement")]
            [SerializeField, Min(0)]
            private float movementSpeed = 3f;
            
            [SerializeField, Min(0)]
            private float weight = 1f;
            
            [Header("Sensor")]
            [SerializeField, Min(0)]
            private float sensorDistance = 10f;
            
            [Space]
            [SerializeField]
            private AttackSettings attackSettings;

            public float HealthPoints => healthPoints;

            public float MovementSpeed => movementSpeed;
            public float StaminaPoints => staminaPoints;
            
            public float SensorDistance => sensorDistance;

            public AttackSettings AttackSettings => attackSettings;

            public float Weight => weight;
        }
    }
}