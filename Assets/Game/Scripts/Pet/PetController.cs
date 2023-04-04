using System.Collections.Generic;
using UnityEngine;

namespace Game.Pet
{
    [RequireComponent(typeof(PetFollowing))]
    public class PetController : MonoBehaviour
    {
        private PetFollowing _following;

        private void Awake()
            => _following = GetComponent<PetFollowing>();

        public void SetFollowingPositions(IList<Transform> target)
            => _following.SetFollowingPoints(target);

        public void ResetPosition()
            => _following.ResetPosition();
    }
}