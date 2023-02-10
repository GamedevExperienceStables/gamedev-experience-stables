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
        private int targetNum;
        
        [SerializeField]
        private float sphereColliderShift;
        
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
        public float SphereColliderShift => sphereColliderShift;
        public int TargetNum => targetNum;
    }
    public class MeleeAbility : ActorAbility<MeleeAbilityDefinition>
    {
        private AimAbility _aim;

        public override bool CanActivateAbility()
            => !_aim.IsActive && Owner.GetCurrentValue(CharacterStats.Stamina) >= Mathf.Abs(Definition.StaminaCost.Value);
        
        
        protected override void OnInitAbility()
        {
            _aim = Owner.GetAbility<AimAbility>();
        }

        protected override void OnActivateAbility()
        {
            Owner.ApplyModifier(CharacterStats.Stamina, Definition.StaminaCost);
            Vector3 sphereShift = Owner.Transform.position + Vector3.forward * Definition.SphereColliderShift;
            DebugExtensions.DebugWireSphere(sphereShift, radius: Definition.MeleeRangeRadius);
            Collider[] hitColliders = new Collider[Definition.TargetNum];
            int numColliders = Physics.OverlapSphereNonAlloc(sphereShift, 
                Definition.MeleeRangeRadius, hitColliders, Definition.Mask);
            for (int i = 0; i < numColliders; i++)
            {
                Debug.Log(hitColliders[i].transform.gameObject + "MELEE ATTACKED");
                if (hitColliders[i].transform.gameObject.TryGetComponent(out IActorController destinationOwner))
                    destinationOwner.GetComponent<DamageableController>().Damage(Definition.MeleeDamage);
                Vector3 dir = hitColliders[i].transform.position - Owner.Transform.position;
                dir = dir.normalized * Definition.PushForce;
                hitColliders[i].transform.GetComponent<MovementController>().AddVelocity(dir);
            }
        }
    }
}