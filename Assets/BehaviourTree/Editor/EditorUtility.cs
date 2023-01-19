using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace BehaviourTree.Editor
{
    public static class EditorUtility
    {
        public static BehaviourTree CreateNewTree(string assetName, string folder)
        {

            string path = System.IO.Path.Join(folder, $"{assetName}.asset");
            if (System.IO.File.Exists(path))
            {
                Debug.LogError($"Failed to create behaviour tree asset: Path already exists:{assetName}");
                return null;
            }
            BehaviourTree tree = ScriptableObject.CreateInstance<BehaviourTree>();
            tree.name = assetName;
            AssetDatabase.CreateAsset(tree, path);
            AssetDatabase.SaveAssets();
            EditorGUIUtility.PingObject(tree);
            return tree;
        }

        public static List<T> LoadAssets<T>() where T : UnityEngine.Object
        {
            string[] assetIds = AssetDatabase.FindAssets($"t:{typeof(T).Name}");
            List<T> assets = new List<T>();
            foreach (var assetId in assetIds)
            {
                string path = AssetDatabase.GUIDToAssetPath(assetId);
                T asset = AssetDatabase.LoadAssetAtPath<T>(path);
                assets.Add(asset);
            }
            return assets;
        }

        public static List<string> GetAssetPaths<T>() where T : UnityEngine.Object
        {
            string[] assetIds = AssetDatabase.FindAssets($"t:{typeof(T).Name}");
            List<string> paths = new List<string>();
            foreach (var assetId in assetIds)
            {
                string path = AssetDatabase.GUIDToAssetPath(assetId);
                paths.Add(path);
            }
            return paths;
        }
    }
}
