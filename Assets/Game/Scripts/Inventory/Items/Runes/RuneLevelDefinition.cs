using UnityEngine;

namespace Game.Inventory
{
    [CreateAssetMenu(menuName = "Rune Level")]
    public class RuneLevelDefinition : ScriptableObject
    {
        [SerializeField]
        private Color color;

        public Color Color => color;
    }
}