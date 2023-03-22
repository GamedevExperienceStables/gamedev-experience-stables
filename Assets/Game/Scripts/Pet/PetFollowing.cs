using System.Collections.Generic;
using Game.Utils;
using UnityEngine;

namespace Game.Pet
{
    public class PetFollowing : MonoBehaviour
    {
        private IList<Transform> _followingPoints;

        [Header("Movement")]
        [SerializeField]
        private float minDamping = 0.4f;

        [SerializeField]
        private float maxDamping = 2f;

        [SerializeField]
        private float maxDistance = 4f;

        [Header("Rotation")]
        [SerializeField]
        private float rotationDamping = 0.8f;

        private Vector3 _velocity;
        private Quaternion _rotation;

        private void Update() 
            => Move();

        public void SetFollowingPoints(IList<Transform> points)
            => _followingPoints = points;

        public void ResetPosition()
        {
            if (_followingPoints.Count <= 0) 
                return;

            Transform firstPoint = _followingPoints[0];
            UpdatePositionAndRotation(firstPoint.position, firstPoint.rotation);
        }

        private void Move()
        {
            Transform target = GetClosestPointToFollow();
            Transform self = transform;

            Vector3 newPosition = SmoothMove(self.position, target.position);
            Quaternion newRotation = SmoothRotate(target, self);

            UpdatePositionAndRotation(newPosition, newRotation);
        }

        private Vector3 SmoothMove(Vector3 position, Vector3 targetPosition)
        {
            Vector3 targetDirection = targetPosition - position;
            float sqrDistanceToTarget = targetDirection.sqrMagnitude;
            float sqrMaxDistance = maxDistance * maxDistance;
            float damping = Mathf.Lerp(maxDamping, minDamping, sqrDistanceToTarget / sqrMaxDistance);

            return Vector3.SmoothDamp(position, targetPosition, ref _velocity, damping);
        }

        private Quaternion SmoothRotate(Transform target, Transform self)
        {
            float angleToTarget = Vector3.Angle(self.forward, target.forward);
            if (angleToTarget.AlmostZero())
                return self.rotation;

            Quaternion targetRotation = Quaternion.LookRotation(target.forward);
            Quaternion rotation = MathExtensions.SmoothDamp(self.rotation, targetRotation, ref _rotation, rotationDamping);
            
            return rotation;
        }

        private Transform GetClosestPointToFollow()
        {
            if (_followingPoints.Count == 1)
                return _followingPoints[0];

            float minDistanceSqr = float.MaxValue;
            Transform closestPoint = null;
            foreach (Transform point in _followingPoints)
            {
                float distanceSqr = (point.position - transform.position).sqrMagnitude;
                if (distanceSqr < minDistanceSqr)
                {
                    minDistanceSqr = distanceSqr;
                    closestPoint = point;
                }
            }

            return closestPoint;
        }

        private void UpdatePositionAndRotation(Vector3 position, Quaternion rotation)
            => transform.SetPositionAndRotation(position, rotation);
    }
}