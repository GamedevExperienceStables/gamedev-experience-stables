using System;
using Game.Actors;
using Game.Stats;
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

            CreateStat(CharacterStats.DashMultiplier);
            CreateStat(CharacterStats.DashStaminaMultiplier);
            CreateStat(CharacterStats.MeleeDamageMultiplier);
            CreateStat(CharacterStats.MeleeStaminaMultiplier);
        }

        public void Init(InitialStats initial)
        {
            this.InitStatsHealth(initial.HealthPoints);
            this.InitStatsMana(initial.ManaPoints);
            this.InitStatsStamina(initial.StaminaPoints);
            this.InitStatsMovement(initial.MovementSpeed);

            InitStat(CharacterStats.DashMultiplier, 0);
            InitStat(CharacterStats.DashStaminaMultiplier, 0);
            InitStat(CharacterStats.MeleeDamageMultiplier, 0);
            InitStat(CharacterStats.MeleeStaminaMultiplier, 0);
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

            public float MovementSpeed => movementSpeed;
            public float HealthPoints => healthPoints;
            public float StaminaPoints => staminaPoints;
            public float ManaPoints => manaPoints;
        }
    }
}