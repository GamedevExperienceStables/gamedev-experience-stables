using UnityEngine;

namespace Game.Actors
{
    public class ProjectileAbilityView : MonoBehaviour
    {
        [SerializeField]
        private Transform spawnPoint;

        public Transform SpawnPoint => spawnPoint;
    }
}