using System.Collections.Generic;
using Game.Actors;
using Game.Level;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Enemies
{
    [CreateAssetMenu(menuName = "★ Entities/Enemy")]
    public class EnemyDefinition : ActorDefinition
    {
        [SerializeField]
        private EnemyController prefab;

        [FormerlySerializedAs("loot")]
        [SerializeField]
        private LootBagDefinition lootBag;

        [SerializeField]
        private EnemyStats.InitialStats initialStats;

        [SerializeField, Expandable]
        private List<AbilityDefinition> initialAbilities;
        
        [SerializeField, Expandable]
        private List<AbilityDefinition> abilities;

        public EnemyController Prefab => prefab;
        public LootBagDefinition LootBag => lootBag;
        public EnemyStats.InitialStats InitialStats => initialStats;
        public List<AbilityDefinition> InitialAbilities => initialAbilities;
        public List<AbilityDefinition> Abilities => abilities;
    }
}