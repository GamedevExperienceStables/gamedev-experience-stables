using System.Collections.Generic;
using Game.Actors;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Hero
{
    [CreateAssetMenu(menuName = "Data/Hero")]
    public class HeroDefinition : ScriptableObject
    {
        [SerializeField]
        private HeroController prefab;

        [SerializeField]
        private HeroStats.Settings initialStats;

        [FormerlySerializedAs("defaultAbilities")]
        [SerializeField, Expandable]
        private List<AbilityDefinition> initialAbilities;

        [SerializeField, Expandable]
        private List<AbilityDefinition> abilities;

        public List<AbilityDefinition> InitialAbilities => initialAbilities;
        public List<AbilityDefinition> Abilities => abilities;

        public HeroController Prefab => prefab;
        public HeroStats.Settings InitialStats => initialStats;
    }
}