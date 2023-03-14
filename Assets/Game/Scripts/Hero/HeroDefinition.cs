using System.Collections.Generic;
using Game.Actors;
using UnityEngine;

namespace Game.Hero
{
    [CreateAssetMenu(menuName = "★ Entities/Hero")]
    public class HeroDefinition : ActorDefinition
    {
        [SerializeField]
        private HeroController prefab;

        [SerializeField]
        private HeroStats.InitialStats initialStats;
        
        [SerializeField]
        private List<AbilityDefinition> initialAbilities;

        [SerializeField]
        private List<AbilityDefinition> abilities;

        [SerializeField]
        private GameObject petPrefab;
        
        public List<AbilityDefinition> InitialAbilities => initialAbilities;
        public List<AbilityDefinition> Abilities => abilities;

        public HeroController Prefab => prefab;
        public HeroStats.InitialStats InitialStats => initialStats;
        public GameObject Pet => petPrefab;
    }
}