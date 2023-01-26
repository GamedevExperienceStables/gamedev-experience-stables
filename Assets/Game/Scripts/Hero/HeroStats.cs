using System;
using System.Collections.Generic;
using Game.Stats;
using UnityEngine;

namespace Game.Hero
{
    [Serializable]
    public class HeroStats : IHeroStats
    {
        public CharacterStatWithMax Health { get; } = new();
        public CharacterStatWithMax Stamina { get; } = new();
        public CharacterStatWithMax Mana { get; } = new();
        
        public CharacterStat Movement { get; } = new();

        public CharacterStat HealthRegeneration { get; } = new();
        public CharacterStat ManaRegeneration { get; } = new();
        public CharacterStat StaminaRegeneration { get; } = new();

        public void InitStats(Settings settings)
        {
            Health.Init(settings.HealthPoints);
            Stamina.Init(settings.StaminaPoints);
            Mana.Init(settings.ManaPoints);
            Movement.Init(settings.MovementSpeed);

            HealthRegeneration.Init(settings.HealthRegeneration);
            ManaRegeneration.Init(settings.ManaRegeneration);
            StaminaRegeneration.Init(settings.StaminaRegeneration);
        }


        [Serializable]
        public class Settings
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