using System.Collections.Generic;
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
        
        public float MeleeRangeRadius => meleeRangeRadius;
        public StatModifier MeleeDamage => meleeDamage;
    }
    public class MeleeAbility : ActorAbility<MeleeAbilityDefinition>
    {
        private AimAbility _aim;
        private MeleeAbility _melee;

        public override bool CanActivateAbility()
            => !_aim.IsActive;

        protected override void OnInitAbility()
        {
            _aim = Owner.GetAbility<AimAbility>();
        }

        protected override void OnActivateAbility()
        {
            LayerMask mask = LayerMask.GetMask("Enemy");
            var hits = Physics.OverlapSphere(Owner.Transform.position,
                Definition.MeleeRangeRadius,LayerMasks.Enemy );
            foreach (Collider hit in hits)
            {
                Debug.Log(hit.transform.gameObject + "MELEE ATTACKED");
                hit.transform.gameObject.TryGetComponent<IActorController>(out IActorController destinationOwner);
                destinationOwner.GetComponent<DamageableController>().Damage(Definition.MeleeDamage);
            }
        }
        
        void OnDrawGizmosSelected()
        {
            // Draw a yellow sphere at the transform's position
            Gizmos.color = Color.cyan;
            Debug.Log(Definition.MeleeRangeRadius);
            Gizmos.DrawSphere(Owner.Transform.position, Definition.MeleeRangeRadius);
        }
    }
}