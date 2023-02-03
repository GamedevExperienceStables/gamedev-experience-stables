using Game.Actors.Health;
using Game.Stats;
using Game.Utils;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Actors
{
    
    [CreateAssetMenu(menuName = MENU_PATH + "MeleeAbility")]
    public class MeleeAbilityDefinition : AbilityDefinition<MeleeAbility>
    {
        [FormerlySerializedAs("MeleeAtkArea")]
        [SerializeField]
        private float meleeRangeRadius;

        [FormerlySerializedAs("MeleeDamage")]
        [SerializeField]
        private StatModifier meleeDamage;
        
        [SerializeField]
        private StatModifier staminaCost;
        
        public float MeleeRangeRadius => meleeRangeRadius;
        public StatModifier MeleeDamage => meleeDamage;
        public StatModifier StaminaCost => staminaCost;
    }
    public class MeleeAbility : ActorAbility<MeleeAbilityDefinition>
    {
        private AimAbility _aim;
        private MeleeAbility _melee;

        public override bool CanActivateAbility()
            => !_aim.IsActive && (Owner.GetCurrentValue(CharacterStats.Stamina) >= Mathf.Abs(Definition.StaminaCost.Value));
        
             

        protected override void OnInitAbility()
        {
            _aim = Owner.GetAbility<AimAbility>();
        }

        protected override void OnActivateAbility()
        {
            // to do: change method to non-allocating in the future
            Owner.ApplyModifier(CharacterStats.Stamina, Definition.StaminaCost);
            var hits = Physics.OverlapSphere(Owner.Transform.position,
                Definition.MeleeRangeRadius,LayerMasks.Enemy );
            foreach (Collider hit in hits)
            {
                Debug.Log(hit.transform.gameObject + "MELEE ATTACKED");
                hit.transform.gameObject.TryGetComponent(out IActorController destinationOwner);
                destinationOwner?.GetComponent<DamageableController>().Damage(Definition.MeleeDamage);
            }
        }
        
        void OnDrawGizmosSelected()
        {
            // for debbuging, delete after adding new method
            Gizmos.color = Color.cyan;
            Gizmos.DrawSphere(Owner.Transform.position, Definition.MeleeRangeRadius);
        }
    }
}