using Game.Weapons;
using UnityEngine;

namespace Game.Actors
{
    public class WeaponController : MonoBehaviour
    {
        [SerializeField]
        private ProjectileWeapon currentWeapon;

        public void Fire()
        {
            currentWeapon.SpawnProjectile();
        }
    }
}