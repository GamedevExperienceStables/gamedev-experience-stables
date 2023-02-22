using Game.Actors;
using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using UnityEngine;

namespace Game.Weapons
{
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

        public void SpawnProjectile(Vector3 targetPosition)
        {
            CreateProjectile(targetPosition);
            PlayEffects();
        }

        private void CreateProjectile(Vector3 targetPosition)
        {
            GameObject go = pool.GetPooledGameObject();
            var projectile = go.GetComponent<Projectile>();
            projectile.Init(firePoint, _owner, targetPosition);
            projectile.Show();
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