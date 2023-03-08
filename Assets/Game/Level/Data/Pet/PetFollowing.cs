using System;
using UnityEngine;

namespace Game.Level.Data.Pet
{
    public class PetFollowing : MonoBehaviour
    {
        [SerializeField]
        private Transform heroPosition;
        private Vector3 _petPosition;
        private void Start()
        {
            _petPosition = transform.position;
        }

        void Update()
        {
            transform.position = _petPosition;
        }
    }
}
