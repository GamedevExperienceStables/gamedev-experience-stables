﻿using Game.Actors.Health;
using Game.Stats;
using Game.Utils;
using KinematicCharacterController;
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

        [SerializeField]
        private float pushForce;
        
        [FormerlySerializedAs("MeleeDamage")]
        [SerializeField]
        private StatModifier meleeDamage;
        
        [SerializeField]
        private StatModifier staminaCost;
        
        public float MeleeRangeRadius => meleeRangeRadius;
        public StatModifier MeleeDamage => meleeDamage;
        public StatModifier StaminaCost => staminaCost;
        public float PushForce => pushForce;
    }
    public class MeleeAbility : ActorAbility<MeleeAbilityDefinition>
    {
        private AimAbility _aim;

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
                if (hit.transform.gameObject.TryGetComponent(out IActorController destinationOwner))
                    destinationOwner.GetComponent<DamageableController>().Damage(Definition.MeleeDamage);
                Vector3 dir = hit.transform.position - Owner.Transform.position;
                dir = dir.normalized * Definition.PushForce;
                hit.transform.GetComponent<KinematicCharacterMotor>().MoveCharacter(hit.transform.position + dir);
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