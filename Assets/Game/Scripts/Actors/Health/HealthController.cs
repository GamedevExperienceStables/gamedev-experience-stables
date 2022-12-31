using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using UnityEngine;

namespace Game.Actors.Health
{
    public class HealthController : MonoBehaviour
    {
        [SerializeField]
        private int initialValue;

        [SerializeField]
        private bool destroyOnDeath = true;

        [Header("Feedbacks")]
        [SerializeField]
        private MMF_Player damageFeedback;

        [SerializeField]
        private MMF_Player deathFeedback;

        [MMReadOnly]
        private int _currentValue;

        public int Value => _currentValue;

        private bool _isDamageable = true;

        private void Awake()
        {
            _currentValue = initialValue;
        }


        public void Damage(int damageValue)
        {
            if (!_isDamageable)
            {
                return;
            }
            
            if (_currentValue <= 0 && initialValue > 0)
            {
                return;
            }

            UpdateValue(damageValue);
            PlayDamageFeedback();

            if (_currentValue <= 0)
            {
                Kill();
            }
        }

        private void Kill()
        {
            PreventDamageWhileDead();
            PlayDeathFeedback();

            if (destroyOnDeath)
            {
                Destroy(gameObject);
            }
        }

        private void PlayDamageFeedback()
        {
            if (damageFeedback)
            {
                Instantiate(damageFeedback, transform.position, transform.rotation);
            }
        }
        
        private void PlayDeathFeedback()
        {
            if (deathFeedback)
            {
                Instantiate(deathFeedback, transform.position, transform.rotation);
            }
        }

        private void UpdateValue(int damageValue)
        {
            _currentValue -= damageValue;
            _currentValue = Mathf.Max(0, _currentValue);
        }

        private void PreventDamageWhileDead()
        {
            _isDamageable = true;
        }
    }
}