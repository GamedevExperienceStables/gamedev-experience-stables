using KinematicCharacterController;
using UnityEngine;

namespace Game.Actors
{
    public class MovementController : MonoBehaviour, ICharacterController
    {
        [SerializeField]
        private KinematicCharacterMotor motor;

        [SerializeField]
        private GroundMovement groundMovement;

        [SerializeField]
        private AirMovement airMovement;

        [SerializeField]
        private Rotation rotation;

        [SerializeField]
        private Gravity gravity;

        private Vector3 _movementDirection;
        private Vector3 _lookDirection;
        public float CapsuleRadius => motor.Capsule.radius;

        private void Awake()
        {
            motor.CharacterController = this;

            groundMovement.Init();
        }

        public void SetPositionAndRotation(Vector3 position, Quaternion quaternion)
        {
            motor.SetPositionAndRotation(position, quaternion);
        }

        public void SetMovementSpeed(float maxSpeed)
        {
            groundMovement.SetMaxSpeed(maxSpeed);
        }

        public void ResetMovementSpeed()
        {
            groundMovement.ResetMaxSpeed();
        }

        public void UpdateInputs(Vector3 movementDirection, Vector3 lookDirection)
        {
            _movementDirection = movementDirection;

            if (rotation.rotateOnlyXZ)
            {
                lookDirection.y = 0;
            }

            _lookDirection = lookDirection;
        }

        public void UpdateRotation(ref Quaternion currentRotation, float deltaTime)
        {
            if (_lookDirection.sqrMagnitude > 0f && rotation.sharpness > 0f)
            {
                Vector3 smoothedLookInputDirection = Vector3.Slerp(
                    motor.CharacterForward,
                    _lookDirection,
                    1 - Mathf.Exp(-rotation.sharpness * deltaTime)
                );

                currentRotation = Quaternion.LookRotation(smoothedLookInputDirection, motor.CharacterUp);
            }
        }

        public void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime)
        {
            if (motor.GroundingStatus.IsStableOnGround)
            {
                UpdateGroundedState(ref currentVelocity, deltaTime);
            }
            else
            {
                UpdateAirborneState(ref currentVelocity, deltaTime);
            }
        }

        private void UpdateGroundedState(ref Vector3 currentVelocity, float deltaTime)
        {
            float currentVelocityMagnitude = currentVelocity.magnitude;
            Vector3 effectiveGroundNormal = motor.GroundingStatus.GroundNormal;

            // Reorient velocity on slope
            currentVelocity = motor.GetDirectionTangentToSurface(currentVelocity, effectiveGroundNormal) *
                              currentVelocityMagnitude;

            // Calculate target velocity
            Vector3 inputRight = Vector3.Cross(_movementDirection, motor.CharacterUp);
            Vector3 reorientedInput = Vector3.Cross(effectiveGroundNormal, inputRight).normalized *
                                      _movementDirection.magnitude;
            Vector3 targetMovementVelocity = reorientedInput * groundMovement.MaxSpeed;

            // Smooth movement Velocity
            currentVelocity = Vector3.Lerp(currentVelocity, targetMovementVelocity,
                1f - Mathf.Exp(-groundMovement.Sharpness * deltaTime));
        }

        private void UpdateAirborneState(ref Vector3 currentVelocity, float deltaTime)
        {
            // Add move input
            if (_movementDirection.sqrMagnitude > 0f)
            {
                Vector3 targetMovementVelocity = _movementDirection * airMovement.maxSpeed;

                // Prevent climbing on un-stable slopes with air movement
                if (motor.GroundingStatus.FoundAnyGround)
                {
                    Vector3 perpendicularObstructionNormal = Vector3.Cross(
                        Vector3.Cross(motor.CharacterUp, motor.GroundingStatus.GroundNormal), motor.CharacterUp
                    ).normalized;

                    targetMovementVelocity =
                        Vector3.ProjectOnPlane(targetMovementVelocity, perpendicularObstructionNormal);
                }

                Vector3 velocityDiff =
                    Vector3.ProjectOnPlane(targetMovementVelocity - currentVelocity, gravity.direction);
                currentVelocity += velocityDiff * (airMovement.acceleration * deltaTime);
            }

            ApplyGravity(ref currentVelocity, deltaTime);
            ApplyDrag(ref currentVelocity, deltaTime);
        }

        private void ApplyDrag(ref Vector3 currentVelocity, float deltaTime)
        {
            currentVelocity *= 1f / (1f + airMovement.drag * deltaTime);
        }

        private void ApplyGravity(ref Vector3 currentVelocity, float deltaTime)
        {
            currentVelocity += gravity.direction * deltaTime;
        }

        public void BeforeCharacterUpdate(float deltaTime)
        {
        }

        public void PostGroundingUpdate(float deltaTime)
        {
        }

        public void AfterCharacterUpdate(float deltaTime)
        {
        }

        public bool IsColliderValidForCollisions(Collider coll)
        {
            return true;
        }

        public void OnGroundHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint,
            ref HitStabilityReport hitStabilityReport)
        {
        }

        public void OnMovementHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint,
            ref HitStabilityReport hitStabilityReport)
        {
        }

        public void ProcessHitStabilityReport(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint,
            Vector3 atCharacterPosition,
            Quaternion atCharacterRotation, ref HitStabilityReport hitStabilityReport)
        {
        }

        public void OnDiscreteCollisionDetected(Collider hitCollider)
        {
        }
    }
}