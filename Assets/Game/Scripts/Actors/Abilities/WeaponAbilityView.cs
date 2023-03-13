using System;
using Game.Weapons;
using UnityEngine;

namespace Game.Actors
{
    [Obsolete("Should be used " + nameof(ProjectileAbilityView))]
    public class WeaponAbilityView : MonoBehaviour
    {
        [SerializeField]
        private ProjectileWeapon currentWeapon;

        public ProjectileWeapon CurrentWeapon => currentWeapon;
    }
}