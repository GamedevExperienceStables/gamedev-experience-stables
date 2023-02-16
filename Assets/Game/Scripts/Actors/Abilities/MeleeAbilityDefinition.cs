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

        [SerializeField]
        private float pushForce;
        
        [SerializeField]
        private int maxTargets;
        
        [SerializeField]
        private Vector3 sphereColliderShift;
        
        [FormerlySerializedAs("MeleeDamage")]
        [SerializeField]
        private StatModifier meleeDamage;
        
        [SerializeField]
        private StatModifier staminaCost;
        
        [SerializeField]
        private LayerMask mask;
        
        public float MeleeRangeRadius => meleeRangeRadius;
        public StatModifier MeleeDamage => meleeDamage;
        public StatModifier StaminaCost => staminaCost;
        public float PushForce => pushForce;
        public LayerMask Mask => mask;
        public Vector3 SphereColliderShift => sphereColliderShift;
        public int MaxTargets => maxTargets;
    }
    public class MeleeAbility : ActorAbility<MeleeAbilityDefinition>
    {
        private AimAbility _aim;
        private Collider[] _hitColliders;
        public override bool CanActivateAbility()
            => !_aim.IsActive && Owner.GetCurrentValue(CharacterStats.Stamina) >= Mathf.Abs(Definition.StaminaCost.Value);
        
        
        protected override void OnInitAbility()
        {
            _aim = Owner.GetAbility<AimAbility>();
            _hitColliders = new Collider[Definition.MaxTargets];
        }

        protected override void OnActivateAbility()
        {
            Owner.ApplyModifier(CharacterStats.Stamina, Definition.StaminaCost);
            Vector3 sphereShift = Owner.Transform.position +  Definition.SphereColliderShift;
            #if UNITY_EDITOR
                DebugExtensions.DebugWireSphere(sphereShift, radius: Definition.MeleeRangeRadius);
             #endif
            int numColliders = Physics.OverlapSphereNonAlloc(sphereShift, 
                Definition.MeleeRangeRadius, _hitColliders, Definition.Mask);
            for (int i = 0; i < numColliders; i++)
            {
                Transform hit = _hitColliders[i].transform;
                Debug.Log(hit.gameObject + "MELEE ATTACKED");
                if (hit.gameObject.TryGetComponent(out IActorController destinationOwner))
                    destinationOwner.GetComponent<DamageableController>().Damage(Definition.MeleeDamage);
                Vector3 dir = hit.position - Owner.Transform.position;
                dir = dir.normalized * Definition.PushForce;
                hit.GetComponent<MovementController>().AddVelocity(dir);
            }
        }
    }
}