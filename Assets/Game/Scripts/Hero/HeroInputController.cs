using Game.Actors;
using Game.Cameras;
using Game.Input;
using UnityEngine;
using VContainer;

namespace Game.Hero
{
    [RequireComponent(typeof(MovementController))]
    public class HeroInputController : MonoBehaviour, IActorInputController
    {
        private SceneCamera _sceneCamera;

        private IInputControlGameplay _input;

        private Vector3 _movementDirection;
        private Vector3 _lookDirection;

        private MovementController _movement;
        private ActiveSkillAbility _activeSkill;
        private InteractionAbility _interaction;
        private AimAbility _aim;
        private DashAbility _dash;
        private MeleeAbility _melee;
        private IActorController _owner;
        private ProjectileAbilityView _projectileView;

        private bool _isBlocked;

        private Ray _mouseRay;
        private Vector3 _mouseFirePosition;
        private Vector3 _mouseGroundPosition;

        public Vector3 DesiredDirection => _movementDirection;

        [Inject]
        public void Construct(IInputControlGameplay input, SceneCamera sceneCamera)
        {
            _input = input;
            _sceneCamera = sceneCamera;
        }

        private void Awake()
        {
            _projectileView = GetComponent<ProjectileAbilityView>();
            _movement = GetComponent<MovementController>();
            _owner = GetComponent<IActorController>();
        }

        private void Start()
        {
            _activeSkill = _owner.GetAbility<ActiveSkillAbility>();
            _interaction = _owner.GetAbility<InteractionAbility>();
            _aim = _owner.GetAbility<AimAbility>();
            _dash = _owner.GetAbility<DashAbility>();
            _melee = _owner.GetAbility<MeleeAbility>();
            _input.AimButton.Performed += OnAim;
            _input.AimButton.Canceled += OnAimCanceled;

            _input.FireButton.Performed += OnFire;
            _input.MeleeButton.Performed += OnMelee;
            _input.InteractionButton.Performed += OnInteract;
            _input.DashButton.Performed += OnDash;
        }

        private void OnDestroy()
        {
            _input.AimButton.Performed -= OnAim;
            _input.AimButton.Canceled -= OnAimCanceled;

            _input.FireButton.Performed -= OnFire;
            _input.MeleeButton.Performed -= OnMelee;
            _input.InteractionButton.Performed -= OnInteract;
            _input.DashButton.Performed -= OnDash;
        }

        private void Update()
        {
            HandleInputs();
            Move();
        }

        private void FixedUpdate()
            => UpdateMousePosition();

        public Vector3 GetTargetPosition(bool grounded = false)
            => grounded ? _mouseGroundPosition : _mouseFirePosition;

        private void UpdateMousePosition()
        {
            _mouseRay = _sceneCamera.ScreenPointToRay(_input.MousePosition);
            _mouseFirePosition = GetMousePosition(_projectileView.SpawnPoint.position);
            _mouseGroundPosition = GetMousePosition(transform.position);
        }

        private Vector3 GetMousePosition(Vector3 position)
        {
            var plane = new Plane(Vector3.up, position);
            plane.Raycast(_mouseRay, out float distance);
            return _mouseRay.GetPoint(distance);
        }

        private void OnFire()
        {
            if (_isBlocked) 
                return;
            
            if (!_aim.IsActive)
                return;
            
            _activeSkill.TryActivateAbility();
        }

        private void OnMelee()
        {
            if (_isBlocked) 
                return;

            if (!_aim.IsActive)
                return;
            
            _melee.TryActivateAbility();
        }

        private void OnDash()
        {
            if (_isBlocked) 
                return;
            
            _dash.TryActivateAbility();
        }

        private void OnAim()
        {
            _aim.TryActivateAbility();
        }

        private void OnAimCanceled()
            => _aim.EndAbility();

        private void OnInteract()
            => _interaction.TryActivateAbility();

        private void HandleInputs()
        {
            if (_isBlocked)
            {
                _movementDirection = Vector3.zero;
                return;
            }

            _movementDirection = GetMovementDirection();
            _lookDirection = GetLookDirection(_movementDirection);
        }

        private void Move()
            => _movement.UpdateInputs(_movementDirection, _lookDirection.normalized);

        public void BlockInput(bool isBlocked)
            => _isBlocked = isBlocked;

        private Vector3 GetMovementDirection()
        {
            Vector3 movementDirection = _sceneCamera.TransformDirection(_input.PrimaryMovement);
            movementDirection.y = 0;

            return movementDirection;
        }

        private Vector3 GetLookDirection(Vector3 movementDirection)
        {
            if (!_aim.IsActive)
                return movementDirection;

            if (IsSecondaryMovementPerformed())
                return GetLookDirectionFromSecondaryMovement();

            return GetLookDirectionFromMousePosition();
        }

        private Vector3 GetLookDirectionFromSecondaryMovement()
            => _sceneCamera.TransformDirection(_input.SecondaryMovement);

        private bool IsSecondaryMovementPerformed()
            => _input.SecondaryMovement.sqrMagnitude > 0;

        private Vector3 GetLookDirectionFromMousePosition()
            => _mouseFirePosition - transform.position;
    }
}