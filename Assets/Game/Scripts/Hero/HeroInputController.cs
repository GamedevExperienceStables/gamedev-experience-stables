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
        private WeaponAbility _weapon;
        private InteractionAbility _interaction;
        private AimAbility _aim;
        private DashAbility _dash;
        private MeleeAbility _melee;
        private RecoveryAbility _recovery;
        private IActorController _owner;
        private bool _isBlocked;
        
        [Inject]
        public void Construct(IInputControlGameplay input, SceneCamera sceneCamera)
        {
            _input = input;
            _sceneCamera = sceneCamera;
        }

        private void Awake()
        {
            _movement = GetComponent<MovementController>();
            _owner = GetComponent<IActorController>();
        }

        private void Start()
        {
            _weapon = _owner.GetAbility<WeaponAbility>();
            _interaction = _owner.GetAbility<InteractionAbility>();
            _aim = _owner.GetAbility<AimAbility>();
            _dash = _owner.GetAbility<DashAbility>();
            _melee = _owner.GetAbility<MeleeAbility>();
            _recovery = _owner.GetAbility<RecoveryAbility>();
            StartRecovering();
            _input.AimButton.Performed += OnAim;
            _input.AimButton.Canceled += OnAimCanceled;

            _input.FireButton.Performed += OnFire;
            _input.FireButton.Performed += OnMelee;
            _input.InteractionButton.Performed += OnInteract;
            _input.DashButton.Performed += OnDash;
        }

        private void OnDestroy()
        {
            _input.AimButton.Performed -= OnAim;
            _input.AimButton.Canceled -= OnAimCanceled;

            _input.FireButton.Performed -= OnFire;
            _input.FireButton.Performed -= OnMelee;
            _input.InteractionButton.Performed -= OnInteract;
            _input.DashButton.Performed -= OnDash;

        }

        private void Update()
        {
            HandleInputs();

            Move();
        }

        private void StartRecovering()
        {
            _recovery.TryActivateAbility();
        }
        
        private void OnFire() 
            => _weapon.TryActivateAbility();

        private void OnMelee()
        {
            if (_isBlocked) return;
            _melee.TryActivateAbility();
        }
        
        private void OnDash() 
            => _dash.TryActivateAbility();

        private void OnAim()
        {
            if (_isBlocked) return;
            _aim.TryActivateAbility();
        }

        private void OnAimCanceled() 
            => _aim.EndAbility();

        private void OnInteract()
            => _interaction.TryActivateAbility();

        private void HandleInputs()
        {
            if (_isBlocked) return;
            _movementDirection = GetMovementDirection();
            _lookDirection = GetLookDirection(_movementDirection);
        }

        private void Move()
            => _movement.UpdateInputs(_movementDirection, _lookDirection.normalized);

        public void BlockInput(bool isBlocked)
            =>_isBlocked = isBlocked;
        
        
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
    }
}