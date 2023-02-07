using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

namespace Yapp
{
    public class SelectionExtension
    {
        #pragma warning disable 0414
        PrefabPainterEditor editor;
        #pragma warning restore 0414

        PrefabPainter editorTarget;

        public SelectionExtension(PrefabPainterEditor editor)
        {
            this.editor = editor;
            this.editorTarget = editor.GetPainter();
        }

        public void OnInspectorGUI()
        {
            GUILayout.BeginVertical("box");
            EditorGUILayout.LabelField("Selection", GUIStyles.BoxTitleStyle);

            GUILayout.BeginHorizontal();
            {
                // draw custom components
                if (GUILayout.Button("Few"))
                {
                    SelectRandom(0.25f);
                }

                if (GUILayout.Button("Medium"))
                {
                    SelectRandom( 0.5f);
                }

                if (GUILayout.Button("Many"))
                {
                    SelectRandom(0.75f);
                }

                if (GUILayout.Button("All"))
                {
                    SelectAll();
                }

            }
            GUILayout.EndHorizontal();

            GUILayout.EndVertical();
        }

        #region Remove Container Children

        public void SelectAll()
        {
            GameObject container = editorTarget.container as GameObject;

            List<Transform> list = new List<Transform>();
            foreach (Transform child in container.transform)
            {
                list.Add(child);
            }

            List<GameObject> gameObjectsList = new List<GameObject>();
            foreach (Transform child in list)
            {
                GameObject go = child.gameObject;

                gameObjectsList.Add(go);

            }

            GameObject[] gameObjectsArray = gameObjectsList.ToArray();

            Selection.objects = gameObjectsArray;
        }

        public void SelectRandom( float delta)
        {
            GameObject container = editorTarget.container as GameObject;

            List<Transform> list = new List<Transform>();
            foreach (Transform child in container.transform)
            {
                list.Add(child);
            }

            List<GameObject> gameObjectsList = new List<GameObject>();
            foreach (Transform child in list)
            {
                GameObject go = child.gameObject;

                if (Random.Range(0f, 1f) <= delta)
                {
                    gameObjectsList.Add(go);
                }

            }

            GameObject[] gameObjectsArray = gameObjectsList.ToArray();

            Selection.objects = gameObjectsArray;
        }
        #endregion Remove Container Children

    }
}
