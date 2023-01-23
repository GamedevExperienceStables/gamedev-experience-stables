using Game.Effects;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Inventory
{
    [CreateAssetMenu(menuName = MENU_PATH + "Stone")]
    public class ConsumableDefinition : ItemDefinition
    {
        [FormerlySerializedAs("effect")]
        [FormerlySerializedAs("action")]
        [SerializeField]
        private GameplayEffectDefinition effectDefinition;

        public GameplayEffectDefinition EffectDefinition => effectDefinition;
    }
}