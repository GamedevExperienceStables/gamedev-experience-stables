using System;
using Game.Actors;
using Game.Cameras;
using Game.Input;
using Game.TimeManagement;
using UnityEngine;
using VContainer;

namespace Game.Hero
{
    [RequireComponent(typeof(MovementController))]
    public sealed class HeroInputController : ActorInputController
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

        private Ray _mouseRay;
        private Vector3 _mouseFirePosition;
        private Vector3 _mouseGroundPosition;

        private const float CHECK_FIRE_INTERVAL = 0.3f;
        
        private TimerPool _timers;
        private TimerUpdatable _fireTimer;

        public override Vector3 DesiredDirection => _movementDirection;

        [Inject]
        public void Construct(IInputControlGameplay input, SceneCamera sceneCamera, TimerPool timers)
        {
            _input = input;
            _sceneCamera = sceneCamera;
            _timers = timers;
            
            _fireTimer = _timers.GetTimer(TimeSpan.FromSeconds(CHECK_FIRE_INTERVAL), OnCheckFire, isLooped: true);
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
            _input.FireButton.Canceled += OnFireCanceled;
            
            _input.MeleeButton.Performed += OnMelee;
            _input.InteractionButton.Performed += OnInteract;
            _input.DashButton.Performed += OnDash;
        }

        private void OnDestroy()
        {
            _input.AimButton.Performed -= OnAim;
            _input.AimButton.Canceled -= OnAimCanceled;

            _input.FireButton.Performed -= OnFire;
            _input.FireButton.Canceled -= OnFireCanceled;
            
            _input.MeleeButton.Performed -= OnMelee;
            _input.InteractionButton.Performed -= OnInteract;
            _input.DashButton.Performed -= OnDash;
            
            _timers?.ReleaseTimer(_fireTimer);
        }

        private void Update()
        {
            HandleInputs();
            Move();
        }

        private void FixedUpdate()
            => UpdateMousePosition();

        public override Vector3 GetTargetPosition(bool grounded = false)
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

        private void OnCheckFire()
        {
            if (_input.FireButton.IsDown)
                OnFire();
        }

        private void OnFire()
        {
            _activeSkill.TryActivateAbility();
            _fireTimer.Start();
        }

        private void OnFireCanceled() 
            => _fireTimer.Stop();

        private void OnMelee() 
            => _melee.TryActivateAbility();

        private void OnDash() 
            => _dash.TryActivateAbility();

        private void OnAim() 
            => _aim.TryActivateAbility();

        private void OnAimCanceled()
            => _aim.EndAbility();

        private void OnInteract() 
            => _interaction.TryActivateAbility();

        private void HandleInputs()
        {
            HandleMovementInput();
            HandleRotationInput();
        }

        private void HandleMovementInput()
            => _movementDirection = block.HasAny(InputBlock.Movement) ? Vector3.zero : GetMovementDirection();

        private void HandleRotationInput()
        {
            if (block.HasAny(InputBlock.Rotation))
                return;

            _lookDirection = _aim.IsActive ? GetLookDirection() : _movementDirection;
        }

        private void Move()
            => _movement.UpdateInputs(_movementDirection, _lookDirection.normalized);


        private Vector3 GetMovementDirection()
        {
            Vector3 movementDirection = _sceneCamera.TransformDirection(_input.PrimaryMovement);
            movementDirection.y = 0;

            return movementDirection;
        }

        private Vector3 GetLookDirection()
        {
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