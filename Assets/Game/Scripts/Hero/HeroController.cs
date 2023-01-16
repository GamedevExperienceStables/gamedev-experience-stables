using Game.Actors;
using Game.Cameras;
using Game.Input;
using UnityEngine;

namespace Game.Hero
{
    [RequireComponent(typeof(MovementController))]
    [RequireComponent(typeof(WeaponController))]
    public class HeroController : MonoBehaviour
    {
        [SerializeField]
        private Transform cameraTarget;

        [SerializeField]
        private float aimMaxSpeed = 5f;

        private SceneCamera _sceneCamera;
        private FollowSceneCamera _followCamera;

        private IInputControlGameplay _input;
        private Vector3 _movementDirection;
        private Vector3 _lookDirection;
     
        private MovementController _movement;
        private WeaponController _weapon;
        private InteractionController _interaction;

        public Transform CameraTarget => cameraTarget;

        private void Awake()
        {
            _movement = GetComponent<MovementController>();
            _weapon = GetComponent<WeaponController>();
            _interaction = GetComponent<InteractionController>();
        }

        public void Init(IInputControlGameplay input, SceneCamera sceneCamera, FollowSceneCamera followCamera)
        {
            _followCamera = followCamera;
            _sceneCamera = sceneCamera;
            _input = input;

            _input.AimButton.Performed += OnAim;
            _input.AimButton.Canceled += OnAimCanceled;

            _input.FireButton.Performed += OnFire;
            _input.InteractionButton.Performed += OnInteract;
        }

        private void OnDestroy()
        {
            if (_input is null)
                return;
            
            _input.AimButton.Performed -= OnAim;
            _input.AimButton.Canceled -= OnAimCanceled;

            _input.FireButton.Performed -= OnFire;
            _input.InteractionButton.Performed -= OnInteract;
        }

        private void Update()
        {
            HandleInputs();

            Move();
        }

        private void OnFire()
        {
            if (_input.AimButton.IsDown) 
                _weapon.Fire();
        }

        private void OnAim()
        {
            _followCamera.ZoomOut();

            _movement.SetMovementSpeed(aimMaxSpeed);
        }

        private void OnAimCanceled()
        {
            _followCamera.ZoomReset();

            _movement.ResetMovementSpeed();
        }
        
        private void OnInteract() 
            => _interaction.Interact();

        private void HandleInputs()
        {
            _movementDirection = GetMovementDirection();
            _lookDirection = GetLookDirection(_movementDirection);
        }

        private void Move() 
            => _movement.UpdateInputs(_movementDirection, _lookDirection.normalized);

        private Vector3 GetMovementDirection()
        {
            Vector3 movementDirection = _sceneCamera.TransformDirection(_input.PrimaryMovement);
            movementDirection.y = 0;

            return movementDirection;
        }

        private Vector3 GetLookDirection(Vector3 movementDirection)
        {
            if (!_input.AimButton.IsDown)
                return movementDirection;

            if (IsSecondaryMovementPerformed())
                return GetLookDirectionFromSecondaryMovement();

            return GetLookDirectionFromMousePosition(movementDirection, _input.MousePosition);
        }

        private Vector3 GetLookDirectionFromSecondaryMovement()
            => _sceneCamera.TransformDirection(_input.SecondaryMovement);

        private bool IsSecondaryMovementPerformed()
            => _input.SecondaryMovement.sqrMagnitude > 0;

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
            => _movement.SetPositionAndRotation(position, rotation);
    }
}