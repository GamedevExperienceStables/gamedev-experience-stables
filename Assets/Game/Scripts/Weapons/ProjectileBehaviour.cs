using System.Collections.Generic;
using Cinemachine.Utility;
using Game.Actors;
using Game.Actors.Health;
using Game.Level;
using Game.Stats;
using Game.Utils;
using UnityEngine;
using VContainer;

namespace Game.Weapons
{
    public class ProjectileBehaviour
    {
        private const float RAYCAST_DISTANCE = 5f;
        private const float DEBUG_DURATION = 0.5f;

        private readonly TrapFactory _trapFactory;
        private readonly EffectHandler _effectHandler;
        
        private readonly Collider[] _buffer = new Collider[20];

        [Inject]
        public ProjectileBehaviour(TrapFactory trapFactory, EffectHandler effectHandler)
        {
            _trapFactory = trapFactory;
            _effectHandler = effectHandler;
        }

        public void Execute(Transform source, DamageDefinition definition, CollisionData collision)
        {
            switch (definition)
            {
                case DamageDefinitionSingle single:
                    TryDamageTarget(source, collision.target, single.Damage, single.HitFeedback, single.Effects);
                    break;

                case DamageDefinitionArea area:
                    TryDamageInArea(area, source, collision);
                    break;

                case DamageDefinitionTrap spawn:
                    SpawnObstacle(spawn, source, collision);
                    break;
            }
        }

        private void TryDamageInArea(DamageDefinitionArea definition, Transform source, CollisionData collision)
        {
#if UNITY_EDITOR
            DebugExtensions.DebugWireSphere(collision.hitPoint, radius: definition.Radius, duration: DEBUG_DURATION);
#endif
            int count = Physics.OverlapSphereNonAlloc(collision.hitPoint, definition.Radius, _buffer, definition.Layers,
                QueryTriggerInteraction.Ignore);
            for (int i = 0; i < count; i++)
                TryDamageTarget(source, _buffer[i], definition.Damage, definition.HitFeedback, definition.Effects);
        }

        private void SpawnObstacle(DamageDefinitionTrap definition, Transform source, CollisionData collision)
        {
            Vector3 spawnPoint = collision.hitPoint;
            Vector3 spawnNormal = collision.hitNormal;

            if (definition.SnapToGround)
            {
                var ray = new Ray(spawnPoint, Vector3.down);
                if (Physics.Raycast(ray, out RaycastHit groundHit, RAYCAST_DISTANCE, definition.GroundLayers,
                        QueryTriggerInteraction.Ignore))
                {
                    spawnPoint = groundHit.point;
                    spawnNormal = definition.AlignToGround ? groundHit.normal : Vector3.up;
                }
            }

            Quaternion rotation = Quaternion.FromToRotation(Vector3.up, spawnNormal);
            
            if (definition.FaceInDirection)
                rotation *= Quaternion.LookRotation(source.forward.ProjectOntoPlane(Vector3.up));

            TrapView trap = _trapFactory.Create(definition.SpawnDefinition);
            trap.transform.SetPositionAndRotation(spawnPoint, rotation);
        }

        private void TryDamageTarget(Transform source, Component target, float damage, GameObject hitFeedback,
            IEnumerable<EffectDefinition> effects)
        {
            if (!IsValidTarget(target, out DamageableController damageable))
                return;

            DamageTarget(source, damageable, damage, hitFeedback);
            
            if (target.TryGetComponent(out IActorController actor))
                _effectHandler.ApplyEffects(actor, effects, source);
        }

        private static void DamageTarget(Transform source, DamageableController target, float damage,
            GameObject hitFeedback)
        {
            ApplyDamage(target, damage);
            PlayHitFeedback(source, hitFeedback);
        }

        private static void ApplyDamage(DamageableController damageable, float damage)
            => damageable.Damage(new StatModifier(-damage, StatsModifierType.Flat));

        private static bool IsValidTarget(Component target, out DamageableController damageable)
        {
            if (!target.TryGetComponent(out damageable))
                return false;

            return !damageable.IsInvulnerable;
        }

        private static void PlayHitFeedback(Transform source, GameObject hitFeedback)
        {
            if (hitFeedback)
                Object.Instantiate(hitFeedback, source.position, source.rotation);
        }
    }
}