using System;
using Game.Input;
using Game.Stats;
using KinematicCharacterController;
using UnityEngine;
using UnityEngine.PlayerLoop;

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
        private Vector3 _internalVelocityAdd;

        private KinematicCharacterMotor _motor;
        private IActorController _owner;
        
        [SerializeField]
        private Animator heroAnimator;
        
        public float CapsuleRadius => _motor.Capsule.radius;

        private void Awake()
        {
            _motor = GetComponent<KinematicCharacterMotor>();
            _motor.CharacterController = this;

            _owner = GetComponent<IActorController>();
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
            if (heroAnimator != null)
            {
                heroAnimator.SetFloat("XCoord", _movementDirection.x);
                heroAnimator.SetFloat("YCoord", _movementDirection.z);
            }
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
        
        public void AddVelocity(Vector3 velocity)
        {
            _internalVelocityAdd += velocity;
        }
        
        public void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime)
        {
            float speed = _owner.GetCurrentValue(CharacterStats.MovementSpeed);
            if (_motor.GroundingStatus.IsStableOnGround)
            {
                UpdateGroundedState(ref currentVelocity, speed, deltaTime);
            }
            else
            {
                UpdateAirborneState(ref currentVelocity, speed, deltaTime);
            }

            // ReSharper disable once InvertIf
            if (_internalVelocityAdd.sqrMagnitude > 0f)
            {
                currentVelocity += _internalVelocityAdd;
                _internalVelocityAdd = Vector3.zero;
            }
        }

        private void UpdateGroundedState(ref Vector3 currentVelocity, float speed, float deltaTime)
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
            Vector3 targetMovementVelocity = reorientedInput * speed;

            // Smooth movement Velocity
            currentVelocity = Vector3.Lerp(currentVelocity, targetMovementVelocity,
                1f - Mathf.Exp(-groundMovement.Sharpness * deltaTime));
        }

        private void UpdateAirborneState(ref Vector3 currentVelocity, float speed, float deltaTime)
        {
            // Add move input
            if (_movementDirection.sqrMagnitude > 0f)
            {
                Vector3 targetMovementVelocity = _movementDirection * speed;

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