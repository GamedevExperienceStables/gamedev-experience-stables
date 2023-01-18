using System;
using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using UnityEngine;

namespace Game.Actors.Health
{
    public class HealthController : MonoBehaviour
    {
        [SerializeField]
        private int initialValue;

        [Header("Feedbacks")]
        [SerializeField]
        private MMF_Player damageFeedback;

        [MMReadOnly]
        private int _currentValue;

        public int Value => _currentValue;

        public event Action<int> HealthChanged;

        private bool _isDamageable = true;

        private void Awake()
        {
            _currentValue = initialValue;
        }

        public void Init(int value)
        {
            _currentValue = value;
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
        }

        private void PlayDamageFeedback()
        {
            if (damageFeedback)
            {
                Instantiate(damageFeedback, transform.position, transform.rotation);
            }
        }

        private void UpdateValue(int damageValue)
        {
            _currentValue -= damageValue;
            _currentValue = Mathf.Max(0, _currentValue);

            HealthChanged?.Invoke(_currentValue);
        }

        public void MakeInvulnerable()
            => _isDamageable = false;
    }
}