using System;
using Game.Hero;
using Game.Scripts.Stats;
using UnityEngine;

namespace Game.Scripts.Hero
{
    public class HeroModel
    {
        private CharacterStat _movementSpeed;
        private CharacterStat _maxMovementSpeed;
    
        private CharacterStat _healthPoints;
        private CharacterStat _maxHealthPoints;
    
        private CharacterStat _staminaPoints;
        private CharacterStat _maxStaminaPoints;
    
        private CharacterStat _manaPoints;
        private CharacterStat _maxManaPoints;
    
        private CharacterStat _healthRegeneration;
        private CharacterStat _maxHealthRegeneration;
    
        private CharacterStat _manaRegeneration;
        private CharacterStat _maxManaRegeneration;
    
        private CharacterStat _staminaRegeneration;
        private CharacterStat _maxStaminaRegeneration;
    
    
        public void InitStats(HeroDefinition heroData)
        {
            _movementSpeed = new CharacterStat(heroData.MovementSpeed);
            _healthPoints = new CharacterStat(heroData.HealthPoints);
            _staminaPoints = new CharacterStat(heroData.StaminaPoints);
            _manaPoints = new CharacterStat(heroData.ManaPoints);
            _healthRegeneration = new CharacterStat(heroData.HealthRegeneration);
            _manaRegeneration = new CharacterStat(heroData.ManaRegeneration);
            _staminaRegeneration = new CharacterStat(heroData.StaminaRegeneration);
        
            _maxMovementSpeed = new CharacterStat(heroData.MovementSpeed);
            _maxHealthPoints = new CharacterStat(heroData.HealthPoints);
            _maxStaminaPoints = new CharacterStat(heroData.StaminaPoints);
            _maxManaPoints = new CharacterStat(heroData.ManaPoints);
            _maxHealthRegeneration = new CharacterStat(heroData.HealthRegeneration);
            _maxManaRegeneration = new CharacterStat(heroData.ManaRegeneration);
            _maxStaminaRegeneration = new CharacterStat(heroData.StaminaRegeneration);
        }

        public CharacterStat MovementSpeed
        {
            get => _movementSpeed;
            set => _movementSpeed = value;
        }
    
        public void UpdateHealth(float newValue)
        {
            var newHealth = Mathf.Clamp(_healthPoints.baseValue + newValue, 0, _maxHealthPoints.Value);

            if (Math.Abs(newHealth - _healthPoints.Value) != 0)
            {
                _healthPoints.baseValue = newHealth;
            }
        }
    
        public void UpdateStamina(float newValue)
        {
            var newStamina = Mathf.Clamp(_staminaPoints.baseValue + newValue, 0, _staminaPoints.Value);

            if (Math.Abs(newStamina - _staminaPoints.Value) != 0)
            {
                _staminaPoints.baseValue = newStamina;
            }
        }
        
    }
}
