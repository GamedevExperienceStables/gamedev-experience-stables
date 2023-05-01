using System.Collections.Generic;
using Cinemachine.Utility;
using Game.Actors;
using Game.Actors.Health;
using Game.Level;
using Game.Utils;
using MoreMountains.Tools;
using UnityEngine;
using VContainer;

namespace Game.Weapons
{
    public class ProjectileHandler
    {
        private const float DEBUG_DURATION = 0.5f;

        private readonly TrapFactory _trapFactory;
        private readonly EffectHandler _effectHandler;

        private readonly Collider[] _buffer = new Collider[20];

        [Inject]
        public ProjectileHandler(TrapFactory trapFactory, EffectHandler effectHandler)
        {
            _trapFactory = trapFactory;
            _effectHandler = effectHandler;
        }

        public void Execute(Transform source, DamageDefinition definition, CollisionData collision)
        {
            switch (definition)
            {
                case DamageDefinitionSingle single:
                    TryDamageTarget(source, collision.target, single.Damage, single.HitFeedback, single.Effects, single.PushForce);
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
                TryDamageTarget(source, _buffer[i], definition.Damage, definition.HitFeedback, definition.Effects, definition.PushForce);
        }

        private void SpawnObstacle(DamageDefinitionTrap definition, Transform source, CollisionData collision)
        {
            Vector3 spawnPoint = collision.hitPoint;
            Vector3 spawnNormal = collision.hitNormal;

            if (!IsValidCollisionToSpawn(definition, collision, spawnNormal))
                return;

            bool hasGround = GroundExistsWithinMaxDistance(definition, spawnPoint, out RaycastHit groundHit);
            if (!hasGround)
                return;

            if (definition.SnapToGround)
            {
                if (!IsValidGroundAngle(groundHit.normal, definition.MaxGroundAngle))
                    return;

                SnapToGround(definition, groundHit, ref spawnPoint, ref spawnNormal);
            }

            Quaternion rotation = Quaternion.FromToRotation(Vector3.up, spawnNormal);

            if (definition.FaceInDirection)
                FaceInDirection(source, ref rotation);

            TrapDefinition trapDefinition = definition.SpawnDefinition;
            TrapView trap = _trapFactory.Create(trapDefinition.Prefab, trapDefinition);
            trap.transform.SetPositionAndRotation(spawnPoint, rotation);
        }

        private static bool GroundExistsWithinMaxDistance(DamageDefinitionTrap definition, Vector3 spawnPoint,
            out RaycastHit groundHit)
        {
            bool foundGround = Physics.Raycast(spawnPoint, Vector3.down, out groundHit, definition.MaxGroundDistance,
                definition.GroundLayers, QueryTriggerInteraction.Ignore);
            return foundGround;
        }

        private static bool IsValidCollisionToSpawn(DamageDefinitionTrap definition, CollisionData collision,
            Vector3 spawnNormal)
        {
            bool isValidLayer = definition.VerticalCollisionLayers.MMContains(collision.target.gameObject);
            return isValidLayer || IsValidGroundAngle(spawnNormal, definition.MaxGroundAngle);
        }

        private static void FaceInDirection(Transform source, ref Quaternion rotation)
            => rotation *= Quaternion.LookRotation(source.forward.ProjectOntoPlane(Vector3.up));

        private static void SnapToGround(DamageDefinitionTrap definition, RaycastHit groundHit, ref Vector3 origin,
            ref Vector3 normal)
        {
            origin = groundHit.point;
            normal = definition.AlignToGround ? groundHit.normal : Vector3.up;
        }

        private static bool IsValidGroundAngle(Vector3 normal, float maxAngle)
        {
            float angle = Vector3.Angle(normal, Vector3.up);
            return angle < maxAngle;
        }

        private void TryDamageTarget(Transform source, Component target, float damage, GameObject hitFeedback,
            IEnumerable<EffectDefinition> effects, float pushForce)
        {
            if (!IsValidTarget(target, out DamageableController damageable))
                return;

            DamageTarget(source, damageable, damage, hitFeedback);

            if (target.TryGetComponent(out IActorController actor))
                _effectHandler.ApplyEffects(actor, effects, source);

            if (pushForce != 0 && target.TryGetComponent(out MovementController movement)) 
                AddPushForce(source, movement, pushForce);
        }

        private static void AddPushForce(Transform source, MovementController target, float pushForce)
        {
            Vector3 dir = target.transform.position - source.position;
            dir = dir.normalized * pushForce;
            
            target.AddVelocity(dir);
        }

        private static void DamageTarget(Transform source, DamageableController target, float damage,
            GameObject hitFeedback)
        {
            ApplyDamage(target, damage);
            PlayHitFeedback(source, hitFeedback);
        }

        private static void ApplyDamage(DamageableController damageable, float damage)
            => damageable.Damage(damage);

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