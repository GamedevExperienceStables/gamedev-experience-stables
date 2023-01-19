using UnityEngine;

namespace Game.Hero
{
    [CreateAssetMenu(menuName = "Data/Hero")]
    public class HeroDefinition : ScriptableObject
    {
        [SerializeField]
        private HeroController prefab;

        [SerializeField]
        private float movementSpeed = 10f;

        [SerializeField]
        private float healthPoints = 10f;
        
        [SerializeField]
        private float staminaPoints = 10f;
        
        [SerializeField]
        private float manaPoints = 10f;
        
        [SerializeField]
        private float healthRegeneration = 1f;
        
        [SerializeField]
        private float manaRegeneration = 1f;
        
        [SerializeField]
        private float staminaRegeneration = 1f;
        
        public HeroController Prefab => prefab;
        public float MovementSpeed => movementSpeed;
        public float HealthPoints => healthPoints;
        public float StaminaPoints => staminaPoints;
        public float ManaPoints => manaPoints;
        public float HealthRegeneration => healthRegeneration;
        public float ManaRegeneration => manaRegeneration;
        public float StaminaRegeneration => staminaRegeneration;
    }
}