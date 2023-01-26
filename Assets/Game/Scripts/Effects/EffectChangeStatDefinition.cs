using Game.Actors;
using Game.Stats;
using UnityEngine;

namespace Game.Effects
{
    [CreateAssetMenu(menuName = "Data/Effects/Restore Stat")]
    public class EffectChangeStatDefinition : GameplayEffectDefinition
    {
        [SerializeField]
        private CharacterStats stat;

        [SerializeField]
        private float value;

        public float Value => value;
        public CharacterStats Stat => stat;
    }
    
    public class EffectChangeStat : GameplayEffect<EffectChangeStatDefinition>
    {
        public override void Execute(IActorController target, IActorController instigator)
        {
            // if (target.HasStats(definition.Stat))
            //     target.ChangeStat(definition.Stat, definition.Value);
        }
    }
}