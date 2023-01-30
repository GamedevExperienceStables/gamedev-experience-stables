using System;
using Game.Actors;
using UnityEngine;

namespace Game.Hero
{
    public class HeroStats : ActorStats
    {
        public HeroStats()
        {
            this.CreateStatsHealth();
            this.CreateStatsMana();
            this.CreateStatsStamina();
            this.CreateStatsMovement();
        }

        public void Init(InitialStats initial)
        {
            this.InitStatsHealth(initial.HealthPoints);
            this.InitStatsMana(initial.ManaPoints);
            this.InitStatsStamina(initial.StaminaPoints);
            this.InitStatsMovement(initial.MovementSpeed);
        }


        [Serializable]
        public class InitialStats
        {
            [SerializeField]
            private float healthPoints = 100f;

            [SerializeField]
            private float staminaPoints = 100f;

            [SerializeField]
            private float manaPoints = 100f;

            [Header("Movement")]
            [SerializeField]
            private float movementSpeed = 6f;

            [Header("Regeneration")]
            [SerializeField]
            private float healthRegeneration = 1f;

            [SerializeField]
            private float manaRegeneration = 1f;

            [SerializeField]
            private float staminaRegeneration = 1f;

            public float MovementSpeed => movementSpeed;
            public float HealthPoints => healthPoints;
            public float StaminaPoints => staminaPoints;
            public float ManaPoints => manaPoints;
            public float HealthRegeneration => healthRegeneration;
            public float ManaRegeneration => manaRegeneration;
            public float StaminaRegeneration => staminaRegeneration;
        }
    }
}