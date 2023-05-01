using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Level
{
    [CreateAssetMenu(menuName = MENU_PATH + "Object")]
    public class TrapDefinition : ScriptableObject
    {
        protected const string MENU_PATH = "★ Trap/";
        
        [FormerlySerializedAs("prefabDeprecated")]
        [Space]
        [SerializeField]
        private TrapView prefab;

        [Space]
        [SerializeField, Min(0)]
        private float lifetime;

        [SerializeField, Min(0)]
        private float size = 1f;

        public TrapView Prefab => prefab;

        public float Size => size;
        public float Lifetime => lifetime;
    }
}