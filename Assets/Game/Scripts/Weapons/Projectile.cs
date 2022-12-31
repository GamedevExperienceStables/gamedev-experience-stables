using System.Linq;
using Game.Actors.Damage;
using Game.Utils;
using MoreMountains.Feedbacks;
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

        [FormerlySerializedAs("deathFeedback")]
        [SerializeField]
        private MMF_Player deathFeedbackPrefab;

        [SerializeField]
        private DamageDealer damageDealer;

        private float _currentSpeed;

        private Vector3 _velocity;

        private float _timer;

        private readonly RaycastHit[] _hits = new RaycastHit[1];

        private void Update()
        {
            CalculateSpeed();
            UpdateLifeTimer();
        }

        private void FixedUpdate()
        {
            if (CheckCollision(out RaycastHit hit))
            {
                DestroyProjectile(hit);
                return;
            }

            Move();
        }

        private bool CheckCollision(out RaycastHit hit)
        {
            Transform t = transform;
            int count = Physics.RaycastNonAlloc(t.position, t.forward, _hits, raycastMaxDistance);
            if (count <= 0)
            {
                hit = default;
                return false;
            }

            hit = _hits.First();
            return true;
        }

        private void DestroyProjectile(RaycastHit hit)
        {
            if (damageDealer.TryDealDamage(hit.transform))
            {
                Complete();
            }
            else
            {
                DestroyOnCollision(hit.point, hit.normal);
            }
        }

        private void DestroyOnCollision(Vector3 position, Vector3 normal)
        {
            PlayDeathFeedback(position, normal);
            Complete();
        }

        public void Init(Transform startPoint)
        {
            transform.SetPositionAndRotation(startPoint.position, startPoint.rotation);

            _timer = lifeTime;
            _currentSpeed = acceleration > 0 ? 0 : maxSpeed;
        }

        private void Complete()
        {
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
            {
                Complete();
            }
        }

        private void PlayDeathFeedback(Vector3 position, Vector3 normal)
        {
            if (deathFeedbackPrefab)
            {
                MMF_Player instance = Instantiate(deathFeedbackPrefab, position, Quaternion.LookRotation(normal));
                instance.PlayFeedbacks();
            }
        }
    }
}