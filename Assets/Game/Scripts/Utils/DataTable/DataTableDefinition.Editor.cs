#if UNITY_EDITOR
using NaughtyAttributes;
using UnityEditor;

namespace Game.Utils.DataTable
{
    public abstract partial class DataTableDefinition<T>
    {
        [Button]
        public void CollectAll()
        {
            items.Clear();

            foreach (string guid in AssetDatabase.FindAssets($"t:{typeof(T)}"))
            {
                T asset = GetAsset(guid);
                items.Add(asset);
            }
        }

        private static T GetAsset(string guid)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
            var asset = AssetDatabase.LoadAssetAtPath<T>(assetPath);
            return asset;
        }
    }
}
#endif