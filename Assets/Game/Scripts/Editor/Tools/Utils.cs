using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Game.Editor.Tools
{
    public static class Utils
    {
        public static Bounds GetSelectionBounds(IEnumerable<Transform> selection)
        {
            var renderers = new List<Renderer>(8);
            var bounds = new Bounds();

            bool boundsInitialized = false;

            foreach (Transform selected in selection)
            {
                if (AssetDatabase.Contains(selected.gameObject))
                    continue;

                renderers.Clear();
                selected.GetComponentsInChildren(renderers);

                for (int j = renderers.Count - 1; j >= 0; j--)
                {
                    if (boundsInitialized)
                        bounds.Encapsulate(renderers[j].bounds);
                    else
                    {
                        bounds = renderers[j].bounds;
                        boundsInitialized = true;
                    }
                }
            }

            return bounds;
        }

        public static GameObject CreateGameObject(Transform parent, Vector3 position, string name = "Game Object")
        {
            var go = new GameObject(name);
            Transform transform = go.transform;
            
            transform.SetParent(parent, false);
            transform.SetSiblingIndex(parent.GetSiblingIndex());
            
            transform.position = position;

            return go;
        }
        
    }
}