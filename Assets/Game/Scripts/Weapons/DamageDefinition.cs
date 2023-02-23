using Game.Actors.Health;
using UnityEngine;

namespace Game.Weapons
{
    public abstract class DamageDefinition : ScriptableObject
    {
        public virtual bool TryDealDamage(Transform source, Transform target, Vector3 hitPoint)
        {
            if (!target.TryGetComponent(out DamageableController damageable))
                return false;

            if (damageable.IsInvulnerable)
                return false;

            OnDamage(source, damageable, hitPoint);

            return true;
        }

        protected abstract void OnDamage(Transform source, DamageableController damageable, Vector3 hitPoint);
    }
}