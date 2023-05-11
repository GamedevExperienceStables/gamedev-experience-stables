using Game.Actors;
using Game.Utils;
using UnityEngine;

namespace Game.Weapons
{
    public class TargetingHandler
    {
        private readonly Collider[] _colliders = new Collider[10];

        public Vector3 GetTargetPosition(TargetingDefinition targeting, Transform source,
            IActorInputController input, Transform spawnPoint)
        {
            Vector3 position = source.position;

            bool groundedPosition = targeting.RelativeToGround;
            Vector3 targetPosition = input.GetTargetPosition(groundedPosition);

            Vector3 origin = spawnPoint.position;
            if (groundedPosition)
                origin.y = position.y;

            if (!targeting.AllowTargetAbove)
                targetPosition.y = Mathf.Min(targetPosition.y, origin.y);

            float sqrDistance = (targetPosition - position).sqrMagnitude;
            bool allowTargetBelow = targeting.AllowTargetBelow;
            float minDistanceBelow = targeting.MinDistanceToTargetBelow;
            if (allowTargetBelow && minDistanceBelow > 0)
            {
                float sqrMinDistance = minDistanceBelow * minDistanceBelow;
                allowTargetBelow = sqrDistance >= sqrMinDistance;
            }

            if (!allowTargetBelow)
                targetPosition.y = Mathf.Max(targetPosition.y, origin.y);

            float minDistanceToTarget = targeting.MinDistanceToTarget;
            if (minDistanceToTarget > 0)
            {
                float sqrMinDistance = minDistanceToTarget * minDistanceToTarget;
                if (sqrMinDistance >= sqrDistance)
                    targetPosition = origin + spawnPoint.forward * minDistanceToTarget;
            }

            targetPosition += targeting.TargetPositionOffset;

            ApplyHelper(targeting.Helper, ref targetPosition);

            return targetPosition;
        }

        private void ApplyHelper(TargetingHelper helper, ref Vector3 targetPosition)
        {
            if (helper.power <= 0 || helper.radius <= 0)
                return;

            int count = Physics.OverlapSphereNonAlloc(targetPosition, helper.radius, _colliders,
                helper.layerMask, QueryTriggerInteraction.Ignore);
            if (count == 0)
                return;

            Vector3 realTargetPosition = targetPosition.FindClosestCollider(_colliders, count);
            realTargetPosition = realTargetPosition.WithY(targetPosition.y);

            targetPosition = Vector3.Lerp(targetPosition, realTargetPosition, helper.power);
        }
    }
}