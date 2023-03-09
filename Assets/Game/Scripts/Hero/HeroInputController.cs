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
        private bool _isBlocked;
        private Vector3 _mousePosition;
        private Plane _plane;
        [SerializeField]
        private GameObject aimSpriteTest;
        
        
        public Vector3 DesiredDirection => _movementDirection;
        
        [Inject]
        public void Construct(IInputControlGameplay input, SceneCamera sceneCamera)
        {
            _input = input;
            _sceneCamera = sceneCamera;
        }

        private void Awake()
        {
            _plane = new Plane(Vector3.up, new Vector3(0, 0, 0));
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
            Aiming();
        }
        
        private void Aiming()
        {
            if (_aim.IsActive)
            {
                if (!aimSpriteTest.activeSelf)
                    aimSpriteTest.SetActive(true);
                
                Ray ray = _sceneCamera.ScreenPointToRay(_input.MousePosition);
                if (_plane.Raycast(ray, out float distance))
                {
                    Vector3 position = transform.position;
                    float x = _mousePosition.x - position.x;
                    float z = _mousePosition.z - position.z;
                    Vector3 newPosition = position + new Vector3(x, 0, z).normalized * 3; 
                    _mousePosition = ray.GetPoint(distance);
                    aimSpriteTest.transform.position = 
                        new Vector3(newPosition.x, aimSpriteTest.transform.position.y, newPosition.z);
                }
            }
            else
            {
                aimSpriteTest.SetActive(false);
            }
        }
        
        public Vector3 GetRealMousePosition() => _mousePosition;

        private void OnFire() 
            => _activeSkill.TryActivateAbility();

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