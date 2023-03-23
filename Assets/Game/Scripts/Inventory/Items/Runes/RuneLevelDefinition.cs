using UnityEngine;
using UnityEngine.Localization;

namespace Game.Inventory
{
    [CreateAssetMenu(menuName = "Rune Level")]
    public class RuneLevelDefinition : ScriptableObject
    {
        [SerializeField]
        private LocalizedString text;
        
        [SerializeField]
        private Color color;

        public LocalizedString Text => text;
        
        public Color Color => color;
    }
}