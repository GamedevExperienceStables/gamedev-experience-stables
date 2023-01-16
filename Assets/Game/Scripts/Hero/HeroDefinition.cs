using UnityEngine;

namespace Game.Hero
{
    [CreateAssetMenu(menuName = "Data/Hero")]
    public class HeroDefinition : ScriptableObject
    {
        [SerializeField]
        private HeroController prefab;

        [SerializeField]
        private float groundMovementSpeed = 10f;

        public HeroController Prefab => prefab;

        public float GroundMovementSpeed => groundMovementSpeed;
    }
}