using UnityEditor;
using UnityEngine;

namespace Game.Editor.Tools
{
    public class CreateEmptyParentAtCenter
    {
        private const string MENU_PATH = "GameObject/Create Empty Parent (at center)";
        
        [MenuItem(MENU_PATH, priority = 0, validate = true)]
        private static bool Validation(MenuCommand command)
            => Selection.transforms.Length > 0;

        [MenuItem(MENU_PATH, priority = 0)]
        private static void CreateEmptyParent(MenuCommand command)
        {
            // This happens when this button is clicked via hierarchy's right click context menu
            // and is called once for each object in the selection. We don't want that, we want
            // the function to be called only once so that there aren't multiple empty parents 
            // generated in one call
            if (command.context)
            {
                EditorApplication.update -= CallCreateEmptyParentOnce;
                EditorApplication.update += CallCreateEmptyParentOnce;

                return;
            }

            var selection = Selection.transforms;
            if (selection.Length == 0)
                return;

            Bounds bounds = Utils.GetSelectionBounds(selection);
            
            Transform group = CreateGroup(bounds.center);
            
            foreach (Transform selected in selection)
            {
                if (!AssetDatabase.Contains(selected.gameObject))
                    Undo.SetTransformParent(selected, group, "Create Empty Parent");
            }

            Selection.activeTransform = group;
        }

        private static Transform CreateGroup(Vector3 position)
        {
            Transform activeParent = Selection.activeTransform.parent;
            GameObject groupObject = Utils.CreateGameObject(activeParent, position);
            //groupTransform.position -= new Vector3( 0f, bounds.extents.y, 0f ); // Move pivot to the bottom

            Undo.RegisterCreatedObjectUndo(groupObject, "Create Empty Parent");

            return groupObject.transform;
        }

        private static void CallCreateEmptyParentOnce()
        {
            EditorApplication.update -= CallCreateEmptyParentOnce;
            CreateEmptyParent(new MenuCommand(null));
        }
    }
}