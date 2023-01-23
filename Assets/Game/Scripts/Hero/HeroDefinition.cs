using Game.Actors;
using UnityEngine;

namespace Game.Hero
{
    [CreateAssetMenu(menuName = "Data/Hero")]
    public class HeroDefinition : ScriptableObject
    {
        [SerializeField]
        private HeroController prefab;

        [SerializeField]
        private HeroStats.Settings initialStats;

        [SerializeField]
        private AimAbility.Settings aim;

        public HeroController Prefab => prefab;
        public HeroStats.Settings InitialStats => initialStats;
        public AimAbility.Settings Aim => aim;
    }
}