using Game.Stats;
using KinematicCharacterController;
using UnityEngine;

namespace Game.Actors
{
    [RequireComponent(typeof(KinematicCharacterMotor))]
    public class MovementController : MonoBehaviour, ICharacterController
    {
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

        private KinematicCharacterMotor _motor;
        private IMovableStats _stats;

        public float CapsuleRadius => _motor.Capsule.radius;

        private void Awake()
        {
            _motor = GetComponent<KinematicCharacterMotor>();
            _motor.CharacterController = this;
        }

        protected void Start()
        {
            var owner = GetComponent<ActorController>();
            _stats = owner.GetStats<IMovableStats>();
        }

        public void SetPositionAndRotation(Vector3 position, Quaternion quaternion)
            => _motor.SetPositionAndRotation(position, quaternion);

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
                    _motor.CharacterForward,
                    _lookDirection,
                    1 - Mathf.Exp(-rotation.sharpness * deltaTime)
                );

                currentRotation = Quaternion.LookRotation(smoothedLookInputDirection, _motor.CharacterUp);
            }
        }

        public void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime)
        {
            if (_motor.GroundingStatus.IsStableOnGround)
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
            Vector3 effectiveGroundNormal = _motor.GroundingStatus.GroundNormal;

            // Reorient velocity on slope
            currentVelocity = _motor.GetDirectionTangentToSurface(currentVelocity, effectiveGroundNormal) *
                              currentVelocityMagnitude;

            // Calculate target velocity
            Vector3 inputRight = Vector3.Cross(_movementDirection, _motor.CharacterUp);
            Vector3 reorientedInput = Vector3.Cross(effectiveGroundNormal, inputRight).normalized *
                                      _movementDirection.magnitude;
            Vector3 targetMovementVelocity = reorientedInput * _stats.Movement.Value;

            // Smooth movement Velocity
            currentVelocity = Vector3.Lerp(currentVelocity, targetMovementVelocity,
                1f - Mathf.Exp(-groundMovement.Sharpness * deltaTime));
        }

        private void UpdateAirborneState(ref Vector3 currentVelocity, float deltaTime)
        {
            // Add move input
            if (_movementDirection.sqrMagnitude > 0f)
            {
                Vector3 targetMovementVelocity = _movementDirection * _stats.Movement.Value;

                // Prevent climbing on un-stable slopes with air movement
                if (_motor.GroundingStatus.FoundAnyGround)
                {
                    Vector3 perpendicularObstructionNormal = Vector3.Cross(
                        Vector3.Cross(_motor.CharacterUp, _motor.GroundingStatus.GroundNormal), _motor.CharacterUp
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