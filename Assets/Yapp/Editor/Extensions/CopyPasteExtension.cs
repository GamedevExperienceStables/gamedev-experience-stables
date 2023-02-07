﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

namespace Yapp
{
    public class CopyPasteExtension
    {
        #pragma warning disable 0414
        PrefabPainterEditor editor;
        #pragma warning restore 0414

        PrefabPainter editorTarget;

        public CopyPasteExtension(PrefabPainterEditor editor)
        {
            this.editor = editor;
            this.editorTarget = editor.GetPainter();
        }

        public void OnInspectorGUI()
        {
            GUILayout.BeginVertical("box");
            EditorGUILayout.LabelField("Copy/Paste", GUIStyles.BoxTitleStyle);

            // transform copy/paste

            // GUILayout.BeginHorizontal();

            if (GUILayout.Button("Copy Transforms"))
            {
                CopyTransforms();
            }
            else if (GUILayout.Button("Paste Transforms"))
            {
                PasteTransforms();
            }

            // GUILayout.EndHorizontal();

            EditorGUILayout.HelpBox("Use in combination with Physics to revert to another state than the previous one.", MessageType.Info);

            GUILayout.EndVertical();
        }

        #region Copy/Paste Transforms

        private void CopyTransforms()
        {
            editorTarget.copyPasteGeometryMap.Clear();

            GameObject container = editorTarget.container as GameObject;

            foreach (Transform child in container.transform)
            {
                GameObject go = child.gameObject;

                if (go == null)
                    continue;

                editorTarget.copyPasteGeometryMap.Add(go.GetInstanceID(), new Geometry(go.transform));

            }

            // logging
            Debug.Log("Copying transforms & rotations: " + editorTarget.copyPasteGeometryMap.Keys.Count);
        }


        private void PasteTransforms()
        {
            // logging
            Debug.Log("Pasting transforms & rotations: " + editorTarget.copyPasteGeometryMap.Keys.Count);

            GameObject container = editorTarget.container as GameObject;

            foreach (Transform child in container.transform)
            {
                GameObject go = child.gameObject;

                if (go == null)
                    continue;

                Geometry geometry = null;

                if (editorTarget.copyPasteGeometryMap.TryGetValue(go.GetInstanceID(), out geometry))
                {
                    go.transform.position = geometry.getPosition();
                    go.transform.rotation = geometry.getRotation();
                }

            }
        }

        #endregion Copy/Paste Transforms
    }
}
