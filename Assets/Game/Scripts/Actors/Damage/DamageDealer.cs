using Game.Actors.Health;
using MoreMountains.Feedbacks;
using UnityEngine;

namespace Game.Actors.Damage
{
    public class DamageDealer : MonoBehaviour
    {
        [SerializeField]
        private int initialValue = 1;

        [SerializeField]
        private MMF_Player hitFeedback;

        public bool TryDealDamage(Transform target)
        {
            if (!target.TryGetComponent(out HealthController health))
            {
                return false;
            }

            if (health.Value <= 0)
            {
                return false;
            }

            PlayHitFeedback();
            ApplyDamage(health);

            return true;
        }

        private void PlayHitFeedback()
        {
            if (hitFeedback)
            {
                Instantiate(hitFeedback, transform.position, transform.rotation);
            }
        }

        private void ApplyDamage(HealthController health)
        {
            health.Damage(initialValue);
        }
    }
}