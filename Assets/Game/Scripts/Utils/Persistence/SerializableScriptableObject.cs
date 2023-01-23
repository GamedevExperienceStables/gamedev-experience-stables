using UnityEngine;

namespace Game.Utils.Persistence
{
    public partial class SerializableScriptableObject : ScriptableObject
    {
        [SerializeField]
        private string id;

        public string Id => id;
    }
}