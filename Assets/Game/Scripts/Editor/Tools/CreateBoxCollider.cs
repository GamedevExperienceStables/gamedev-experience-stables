using UnityEditor;
using UnityEngine;

namespace Game.Editor.Tools
{
    public class CreateBoxCollider
    {
        private const string MENU_PATH = "GameObject/Create Collider From Selection";

        [MenuItem(MENU_PATH, priority = 0, validate = true)]
        private static bool Validation(MenuCommand command)
            => Selection.transforms.Length > 1;


        [MenuItem(MENU_PATH, priority = 0)]
        private static void CreateCollider(MenuCommand command)
        {
            // This happens when this button is clicked via hierarchy's right click context menu
            // and is called once for each object in the selection. We don't want that, we want
            // the function to be called only once so that there aren't multiple empty parents 
            // generated in one call
            if (command.context)
            {
                EditorApplication.update -= CallOnce;
                EditorApplication.update += CallOnce;

                return;
            }

            var selection = Selection.transforms;
            if (selection.Length == 0)
                return;

            Bounds bounds = Utils.GetSelectionBounds(selection);
            Transform collider = CreateCollider(bounds);

            Selection.activeTransform = collider;
        }

        private static Transform CreateCollider(Bounds bounds)
        {
            Transform activeParent = Selection.activeTransform.parent;
            GameObject go = Utils.CreateGameObject(activeParent, bounds.center, "Collider");
            
            var collider = go.AddComponent<BoxCollider>();
            collider.size = bounds.size;

            Undo.RegisterCreatedObjectUndo(go, "Create Collider");

            return collider.transform;
        }


        private static void CallOnce()
        {
            EditorApplication.update -= CallOnce;
            CreateCollider(new MenuCommand(null));
        }
    }
}