using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

namespace Game.Weapons
{
    [CreateAssetMenu(menuName = "Projectile/Base")]
    public class ProjectileDefinition : ScriptableObject
    {
        [SerializeField]
        private Projectile projectilePrefab;

        [SerializeField, Expandable]
        private DamageDefinition[] damages;

        [SerializeField, Min(0.0001f)]
        private float speed = 30f;

        [SerializeField]
        private ProjectileLifetime lifeTime;

        [SerializeField]
        private LayerMask collisionLayers = ~0;

        public Projectile Prefab => projectilePrefab;
        public LayerMask CollisionLayers => collisionLayers;

        public ProjectileLifetime LifeTime => lifeTime;

        public float Speed => speed;

        public IList<DamageDefinition> Damages => damages;
    }
}