using Game.Weapons;
using UnityEngine;

namespace Game.Actors
{
    public class WeaponAbilityView : MonoBehaviour
    {
        [SerializeField]
        private ProjectileWeapon currentWeapon;

        public ProjectileWeapon CurrentWeapon => currentWeapon;
    }
}