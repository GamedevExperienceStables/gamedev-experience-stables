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

        public void SpawnProjectile()
        {
            CreateProjectile();
            PlayEffects();
        }

        private void CreateProjectile()
        {
            GameObject go = pool.GetPooledGameObject();
            var projectile = go.GetComponent<Projectile>();
            projectile.Init(firePoint);
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