using Game.Level;
using UnityEngine;
using UnityEngine.Serialization;

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

        [FormerlySerializedAs("loot")]
        [SerializeField]
        private LootBagDefinition lootBag;

        public EnemyController Prefab => prefab;

        public int Health => health;

        public float MovementSpeed => movementSpeed;

        public LootBagDefinition LootBag => lootBag;
    }
}