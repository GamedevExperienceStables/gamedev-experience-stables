using UnityEngine;

namespace Game.Enemies
{
    [CreateAssetMenu(menuName = "Data/Enemy")]
    public class EnemyDefinition : ScriptableObject
    {
        [SerializeField]
        private EnemyController prefab;

        [SerializeField]
        private int health = 1;

        [SerializeField]
        private float movementSpeed = 5;

        public EnemyController Prefab => prefab;

        public int Health => health;

        public float MovementSpeed => movementSpeed;
    }
}