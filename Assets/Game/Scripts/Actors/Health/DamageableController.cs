using System;
using Game.Level;
using Game.Stats;
using UnityEngine;
using VContainer;

namespace Game.Actors.Health
{
    public class DamageableController : MonoBehaviour
    {
        [SerializeField]
        private GameObject damageFeedback;

        public bool IsInvulnerable { get; private set; }

        private IActorController _owner;
        
        private SpawnPool _spawnPool;
        public event Action DamageFeedback;

        [Inject]
        public void Construct(SpawnPool spawnPool) 
            => _spawnPool = spawnPool;

        private void Awake() 
            => _owner = GetComponent<IActorController>();

        private void Start()
        {
            _owner.SubscribeStatChanged(CharacterStats.Health, OnHealthChanged);
            
            if (damageFeedback)
                _spawnPool.Prewarm(damageFeedback);
        }

        private void OnDestroy()
            => _owner.UnSubscribeStatChanged(CharacterStats.Health, OnHealthChanged);

        private void OnHealthChanged(StatValueChange change)
        {
            if (Mathf.Floor(change.newValue) >= Mathf.Floor(change.oldValue))
                return;

            DamageFeedback?.Invoke();
            PlayDamageFeedback();
        }

        public void Damage(float damage)
        {
            if (IsInvulnerable)
                return;

            ApplyDamage(damage);
        }

        private void PlayDamageFeedback()
        {
            if (!damageFeedback) 
                return;
            
            Transform self = transform;
            _spawnPool.Spawn(damageFeedback, self.position, self.rotation);
        }

        private void ApplyDamage(float damage)
            => _owner.ApplyModifier(CharacterStats.Health, -damage);

        public void MakeInvulnerable()
            => IsInvulnerable = true;

        public void MakeVulnerable()
            => IsInvulnerable = false;
    }
}