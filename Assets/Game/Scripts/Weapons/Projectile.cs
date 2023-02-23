using System;
using System.Collections.Generic;
using System.Linq;
using Game.Actors;
using Game.Utils;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Weapons
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField]
        private float maxSpeed = 200f;

        [SerializeField]
        private float acceleration;

        [SerializeField]
        private float lifeTime = 3f;

        [SerializeField]
        private float raycastMaxDistance = 0.4f;
        
        [SerializeField]
        private float raycastRadius = 0.1f;

        [SerializeField]
        private LayerMask collisionLayerMask;

        [FormerlySerializedAs("deathFeedback")]
        [SerializeField]
        private GameObject deathFeedbackPrefab;

        private float _currentSpeed;

        private Vector3 _velocity;

        private float _timer;

        private readonly RaycastHit[] _hits = new RaycastHit[5];
        private IActorController _owner;
        
        private ProjectileLifetime _lifeTime;
        private IList<DamageDefinition> _damages;

        public event Action<Projectile> Completed;

        private void Update()
        {
            CalculateSpeed();
            UpdateLifeTimer();

            Move();
        }

        private void FixedUpdate()
        {
            if (DetectCollisions(out RaycastHit hit))
            {
                DestroyProjectile(hit.transform, hit.point, hit.normal);
            }
        }

        private bool DetectCollisions(out RaycastHit hit)
        {
            Transform t = transform;
            var ray = new Ray(t.position, t.forward);
            int count = Physics.RaycastNonAlloc(ray, _hits, raycastMaxDistance, collisionLayerMask,
                QueryTriggerInteraction.Ignore);
            if (count <= 0)
            {
                hit = default;
                return false;
            }

            hit = _hits.First();
            return true;
        }

        private void DestroyProjectile(Transform target, Vector3 hitPoint, Vector3 hitNormal)
        {
            bool damaged = false;

            foreach (DamageDefinition damage in _damages)
                damaged |= damage.TryDealDamage(transform, target, hitPoint);

            if (damaged)
                Complete();
            else
                DestroyOnCollision(hitPoint, hitNormal);
        }

        private void DestroyOnCollision(Vector3 position, Vector3 normal)
        {
            PlayDeathFeedback(position, normal);
            Complete();
        }

        public void Init(Transform startPoint, IActorController owner, Vector3 targetPosition)
        {
            transform.SetPositionAndRotation(startPoint.position, startPoint.rotation);
            _timer = lifeTime;
            _currentSpeed = acceleration > 0 ? 0 : maxSpeed;
        }
        

        public void Init(LayerMask collisionLayers, float speed, ProjectileLifetime lifeTime,
            IList<DamageDefinition> damages)
        {
            collisionLayerMask = collisionLayers;
            maxSpeed = speed;
            _lifeTime = lifeTime;
            _damages = damages;
        }

        public void Fire(Transform spawnPoint, Vector3 targetPosition)
        {
            Vector3 startPosition = spawnPoint.position;
            float coord1 = targetPosition.x - startPosition.x;
            float coord2 = targetPosition.z - startPosition.z;
            float angle = Mathf.Atan2(coord1, coord2) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.Euler(new Vector3(0, angle, 0));
            
            transform.SetPositionAndRotation(startPosition, rotation);

            _timer = _lifeTime.Duration;
            _currentSpeed = acceleration > 0 ? 0 : maxSpeed;

            Show();
        }

        private void Complete()
        {
            Completed?.Invoke(this);
            Hide();
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        private void Move()
        {
            transform.Translate(_velocity, Space.World);
        }

        private void CalculateSpeed()
        {
            _velocity = transform.forward * (_currentSpeed * Time.deltaTime);
            if (!MathExtensions.AlmostEquals(_currentSpeed, maxSpeed))
            {
                _currentSpeed += acceleration * Time.deltaTime;
            }
        }

        private void UpdateLifeTimer()
        {
            _timer -= Time.deltaTime;
            if (_timer <= 0f) 
                OnLifetimeEnd();
        }

        private void OnLifetimeEnd()
        {
            switch (_lifeTime.OnEndBehaviour)
            {
                case ProjectileLifetime.EndBehaviour.Execute:
                {
                    Transform t = transform;
                    DestroyProjectile(t, t.position, t.up);
                    break;
                }

                default:
                {
                    Complete();
                    break;
                }
            }
        }

        private void PlayDeathFeedback(Vector3 position, Vector3 normal)
        {
            if (deathFeedbackPrefab) 
                Instantiate(deathFeedbackPrefab, position, Quaternion.LookRotation(normal));
        }
    }
}