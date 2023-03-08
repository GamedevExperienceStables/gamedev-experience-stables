using System;
using UnityEngine;

namespace Game.Level.Data.Pet
{
    public class PetFollowing : MonoBehaviour
    {
        [SerializeField]
        private Transform followingPosition;

        [SerializeField]
        private float speed;
        private Vector3 _petPosition;
        private bool _isChasing;
        

        private void Start()
        {
            _petPosition = transform.position;
        }

        void LateUpdate()
        {
            Chasing(_isChasing);
            if (!_isChasing && Vector3.Distance(transform.position, followingPosition.position) > 2)
            {
                _isChasing = true;
            }
            else if (_isChasing && Vector3.Distance(transform.position, followingPosition.position) < 0.2)
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
            transform.position = Vector3.MoveTowards(transform.position, followingPosition.position,
                speed * Time.deltaTime);
            _petPosition = transform.position;
        }
    }
}
