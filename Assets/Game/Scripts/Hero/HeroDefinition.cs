using System.Collections.Generic;
using Game.Actors;
using Game.Pet;
using UnityEngine;

namespace Game.Hero
{
    [CreateAssetMenu(menuName = "★ Entities/Hero")]
    public class HeroDefinition : ActorDefinition
    {
        [SerializeField]
        private HeroController prefab;

        [SerializeField]
        private PetController petPrefab;

        [SerializeField]
        private HeroStats.InitialStats initialStats;
        
        [SerializeField]
        private List<AbilityDefinition> initialAbilities;

        [SerializeField]
        private List<AbilityDefinition> abilities;

        public List<AbilityDefinition> InitialAbilities => initialAbilities;
        public List<AbilityDefinition> Abilities => abilities;

        public HeroController Prefab => prefab;
        public PetController PetPrefab => petPrefab;

        public HeroStats.InitialStats InitialStats => initialStats;
    }
}