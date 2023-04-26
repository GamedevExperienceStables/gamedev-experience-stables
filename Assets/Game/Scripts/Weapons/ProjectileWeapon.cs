using System;
using Game.Actors;
using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using UnityEngine;

namespace Game.Weapons
{
    [Obsolete("Should be used " + nameof(ProjectileAbility))]
    public class ProjectileWeapon : MonoBehaviour
    {
        [SerializeField]
        private Transform firePoint;

        [SerializeField]
        private MMF_Player recoilFeedbacks;

        [SerializeField]
        private MMSimpleObjectPooler pool;

        private IActorController _owner;

        public void SetOwner(IActorController owner)
            => _owner = owner;

        public void SpawnProjectile()
        {
            CreateProjectile();
            PlayEffects();
        }

        private void CreateProjectile()
        {
            GameObject go = pool.GetPooledGameObject();
            var projectile = go.GetComponent<Projectile>();
        }

        private void PlayEffects()
        {
            if (recoilFeedbacks)
            {
                recoilFeedbacks.PlayFeedbacks();
            }
        }

        public void OnDestroy()
        {
            pool.DestroyObjectPool();
        }
    }
}