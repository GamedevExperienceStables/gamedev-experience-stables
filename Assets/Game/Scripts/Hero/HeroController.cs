using Game.Actors;
using Game.Cameras;
using Game.Input;
using UnityEngine;

namespace Game.Hero
{
    [RequireComponent(typeof(MovementController), typeof(WeaponController))]
    public class HeroController : MonoBehaviour
    {
        private MovementController movement;
        private WeaponController weapon;

        [SerializeField]
        private Transform cameraTarget;

        [SerializeField]
        private float aimMaxSpeed = 5f;

        private SceneCamera _sceneCamera;
        private FollowSceneCamera _followCamera;

        private IInputReader _inputReader;
        private Vector3 _movementDirection;
        private Vector3 _lookDirection;

        public Transform CameraTarget => cameraTarget;

        private void Awake()
        {
            movement = GetComponent<MovementController>();
            weapon = GetComponent<WeaponController>();
        }

        public void Construct(IInputReader inputReader, SceneCamera sceneCamera, FollowSceneCamera followCamera)
        {
            _followCamera = followCamera;
            _sceneCamera = sceneCamera;
            _inputReader = inputReader;

            _inputReader.AimButton.Performed += OnAim;
            _inputReader.AimButton.Canceled += OnAimCanceled;

            _inputReader.FireButton.Performed += OnFire;
        }

        private void OnDestroy()
        {
            _inputReader.AimButton.Performed -= OnAim;
            _inputReader.AimButton.Canceled -= OnAimCanceled;

            _inputReader.FireButton.Performed -= OnFire;
        }

        private void Update()
        {
            HandleInputs();

            Move();
        }

        private void OnFire()
        {
            if (_inputReader.AimButton.IsDown)
            {
                weapon.Fire();
            }
        }

        private void OnAim()
        {
            _followCamera.ZoomOut();

            movement.SetMovementSpeed(aimMaxSpeed);
        }

        private void OnAimCanceled()
        {
            _followCamera.ZoomReset();

            movement.ResetMovementSpeed();
        }

        private void HandleInputs()
        {
            _movementDirection = GetMovementDirection();
            _lookDirection = GetLookDirection(_movementDirection);
        }

        private void Move()
        {
            movement.UpdateInputs(_movementDirection, _lookDirection.normalized);
        }

        private Vector3 GetMovementDirection()
        {
            Vector3 movementDirection = _sceneCamera.TransformDirection(_inputReader.PrimaryMovement);
            movementDirection.y = 0;

            return movementDirection;
        }

        private Vector3 GetLookDirection(Vector3 movementDirection)
        {
            if (!_inputReader.AimButton.IsDown)
            {
                return movementDirection;
            }

            if (IsSecondaryMovementPerformed())
            {
                return GetLookDirectionFromSecondaryMovement();
            }

            return GetLookDirectionFromMousePosition(movementDirection, _inputReader.MousePosition);
        }

        private Vector3 GetLookDirectionFromSecondaryMovement()
        {
            return _sceneCamera.TransformDirection(_inputReader.SecondaryMovement);
        }

        private bool IsSecondaryMovementPerformed()
        {
            return _inputReader.SecondaryMovement.sqrMagnitude > 0;
        }

        private Vector3 GetLookDirectionFromMousePosition(Vector3 movementDirection, Vector2 mousePosition)
        {
            Ray ray = _sceneCamera.ScreenPointToRay(mousePosition);
            var plane = new Plane(Vector3.up, transform.position);
            if (plane.Raycast(ray, out float distance))
            {
                Vector3 hitPosition = ray.GetPoint(distance);
                return hitPosition - transform.position;
            }

            return movementDirection;
        }

        public void SetPositionAndRotation(Vector3 position, Quaternion rotation)
        {
            movement.SetPositionAndRotation(position, rotation);
        }
    }
}