#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Game.Utils.Persistence
{
    public partial class SerializableScriptableObject
    {
        private void Awake()
            => GenerateId();

        private void OnValidate()
            => GenerateId();

        public void Reset()
            => GenerateId();

        private void GenerateId()
        {
            if (!string.IsNullOrEmpty(id))
                return;

            string guid = AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(this));
            if (guid == id)
                return;

            string old = id;
            id = guid;

            if (!string.IsNullOrEmpty(old) && old != id)
                Debug.LogError($"[ID] '{name}': {old} -> {id} (save data may be wrong)");
        }
    }
}
#endif