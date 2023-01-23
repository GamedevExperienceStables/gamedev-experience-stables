using Game.Actors;
using Game.Stats;
using UnityEngine;
using VContainer;
using VContainer.Unity;

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

        public class EffectChangeStat : GameplayEffect
        {
            private readonly EffectChangeStatDefinition _definition;

            [Inject]
            public EffectChangeStat(EffectChangeStatDefinition definition) 
                => _definition = definition;

            public override void Execute(IActorController target, IActorController instigator)
            {
                // if (target.Stats.HasStat(_definition.Stat))
                //     target.Stats.AddValue(_definition.Stat, _definition.Value);
            }
        }
    }
}