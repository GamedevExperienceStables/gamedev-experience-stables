using UnityEngine;

namespace Game.Level
{
    [CreateAssetMenu(menuName = MENU_PATH + "Object")]
    public class TrapDefinition : ScriptableObject
    {
        protected const string MENU_PATH = "★ Trap/";
        
        [SerializeField]
        private TrapView prefab;

        [Space]
        [SerializeField, Min(0.001f)]
        private float lifetime;

        [SerializeField]
        private float size = 1f;

        public TrapView Prefab => prefab;

        public float Size => size;
        public float Lifetime => lifetime;
    }
}