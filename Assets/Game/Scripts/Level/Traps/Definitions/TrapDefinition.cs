using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Level
{
    [CreateAssetMenu(menuName = MENU_PATH + "Object")]
    public class TrapDefinition : ScriptableObject
    {
        protected const string MENU_PATH = "★ Trap/";
        
        [FormerlySerializedAs("prefab")]
        [Space]
        [SerializeField, Obsolete]
        private TrapView prefabDeprecated;

        [Space]
        [SerializeField, Min(0)]
        private float lifetime;

        [SerializeField, Min(0)]
        private float size = 1f;

        public TrapView PrefabDeprecated => prefabDeprecated;

        public float Size => size;
        public float Lifetime => lifetime;
    }
}