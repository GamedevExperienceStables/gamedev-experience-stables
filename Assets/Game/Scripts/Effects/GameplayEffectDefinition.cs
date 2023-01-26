using UnityEngine;

namespace Game.Effects
{
    public abstract class GameplayEffectDefinition : ScriptableObject
    {
        public GameplayEffectDurationPolicy duration = GameplayEffectDurationPolicy.Instant;

        
        
    }
}