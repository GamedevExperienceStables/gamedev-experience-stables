using System;
using System.Collections.Generic;
using CleverCrow.Fluid.UniqueIds;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Editor.UniqueIds
{
    public static class SceneUniqueIdFix
    {
        private const string MENU_PATH = "Window/Fluid/Set Empty Unique ID On Scene";

        [MenuItem(MENU_PATH, true, 10)]
        public static bool Validate()
            => !EditorApplication.isPlaying;

        [MenuItem(MENU_PATH, false, 10)]
        public static void Fix()
        {
            Scene activeScene = SceneManager.GetActiveScene();
            SceneSearch(activeScene);
        }

        private static void SceneSearch(Scene scene)
        {
            var items = FindEmptyIds(scene);
            foreach (UniqueId uniqueId in items)
                FixId(uniqueId);
            
            Debug.Log($"{items.Count} Unique IDs fixed in scene {scene.name}");
        }

        private static void FixId(UnityEngine.Object uniqueId)
        {
            var obj = new SerializedObject(uniqueId);

            SerializedProperty idProp = obj.FindProperty("_id");
            idProp.stringValue = Guid.NewGuid().ToString();

            obj.ApplyModifiedProperties();

            Undo.RecordObject(uniqueId, "Set Unique ID");
        }

        private static List<UniqueId> FindEmptyIds(Scene activeScene)
        {
            List<UniqueId> buffer = new();
            List<UniqueId> results = new();

            foreach (GameObject rootGameObject in activeScene.GetRootGameObjects())
            {
                rootGameObject.GetComponentsInChildren(true, buffer);

                foreach (UniqueId uniqueId in buffer)
                {
                    if (string.IsNullOrEmpty(uniqueId.Id))
                        results.Add(uniqueId);
                }
            }

            return results;
        }
    }
}