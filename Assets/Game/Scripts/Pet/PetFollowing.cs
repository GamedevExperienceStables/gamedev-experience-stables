using UnityEngine;

namespace Game.Pet
{
    public class PetFollowing : MonoBehaviour
    {
        private Transform _followingPosition;

        [SerializeField]
        private float speed;
        private Vector3 _petPosition;
        private bool _isChasing;
        

        public void SetFollowingPosition(Transform heroPetTransform)
        {
            _followingPosition = heroPetTransform;
            _petPosition = heroPetTransform.position;
        }
        
        void LateUpdate()
        {
            Chasing(_isChasing);
            if (!_isChasing && Vector3.Distance(transform.position, _followingPosition.position) > 0.5f)
            {
                _isChasing = true;
            }
            else if (_isChasing && Vector3.Distance(transform.position, _followingPosition.position) < 0.2f)
            {
                _isChasing = false;
            }
            else
            {
                transform.position = _petPosition;
            }
        }

        private void Chasing(bool isChasing)
        {
            if (!isChasing) return;
            var position = transform.position;
            position = Vector3.MoveTowards(position, _followingPosition.position,
                speed * Time.deltaTime);
            transform.position = position;
            _petPosition = position;
        }
    }
}
