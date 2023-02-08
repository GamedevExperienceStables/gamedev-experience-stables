using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Yapp
{
    public class PrefabModuleEditor : ModuleEditorI
    {

#pragma warning disable 0414
        PrefabPainterEditor editor;
        PrefabPainter editorTarget;
#pragma warning restore 0414


        public PrefabTemplateCollection templateCollection;
        private PrefabSettingsTemplate defaultTemplate = ScriptableObject.CreateInstance<PrefabSettingsTemplate>();

        public PrefabModuleEditor(PrefabPainterEditor editor)
        {
            this.editor = editor;
            this.editorTarget = editor.GetPainter();

            LoadTemplateCollection();
        }

        private void LoadTemplateCollection()
        {
            // load the available prefab settings templates
            string[] templateCollectionGuids = AssetDatabase.FindAssets(string.Format("t:{0}", typeof(PrefabTemplateCollection)));

            // check if we have a template collection at all
            if (templateCollectionGuids.Length == 0)
            {
                Debug.LogError(string.Format("No asset of type {0] found", typeof(PrefabTemplateCollection)));
            }

            // use the first one found
            string templateCollectionGuidsFilePath = AssetDatabase.GUIDToAssetPath(templateCollectionGuids[0]);

            // if we have more than 1 colleciton, inform the user that e use the first one
            if (templateCollectionGuids.Length > 1)
            {
                Debug.LogError(string.Format("Multiple template collections found, this isn't supported yet. Using the first one found: {0}", templateCollectionGuidsFilePath));
            }

            templateCollection = AssetDatabase.LoadAssetAtPath(templateCollectionGuidsFilePath, typeof(PrefabTemplateCollection)) as PrefabTemplateCollection;

            if (!templateCollection)
            {
                Debug.LogError(string.Format("Template collection not found: {0}", templateCollectionGuidsFilePath));
            }

        }

        public void OnInspectorGUI()
        {

            GUILayout.BeginVertical("box");
            {

                EditorGUILayout.LabelField("Prefabs", GUIStyles.BoxTitleStyle);

                #region template drop targets
                GUILayout.BeginHorizontal();
                {
                    GUILayout.BeginVertical();
                    {
                        // change background color in case there are no prefabs yet
                        if (editorTarget.prefabSettingsList.Count == 0)
                        {
                            EditorGUILayout.HelpBox("Drop prefabs on the prefab template boxes in order to use them.", MessageType.Info);

                            editor.SetErrorBackgroundColor();
                        }

                        int gridRows = Mathf.CeilToInt((float)templateCollection.templates.Count / Constants.PrefabTemplateGridColumnCount);

                        for (int row = 0; row < gridRows; row++)
                        {
                            GUILayout.BeginHorizontal();
                            {
                                for (int column = 0; column < Constants.PrefabTemplateGridColumnCount; column++)
                                {
                                    int index = column + row * Constants.PrefabTemplateGridColumnCount;

                                    PrefabSettingsTemplate template = index < templateCollection.templates.Count ? templateCollection.templates[index] : defaultTemplate;

                                    // drop area
                                    Rect prefabDropArea = GUILayoutUtility.GetRect(0.0f, 34.0f, GUIStyles.DropAreaStyle, GUILayout.ExpandWidth(true));

                                    bool hasDropArea = index < templateCollection.templates.Count;
                                    if (hasDropArea)
                                    {
                                        // drop area box with background color and info text
                                        GUI.color = GUIStyles.DropAreaBackgroundColor;
                                        GUI.Box(prefabDropArea, template.templateName, GUIStyles.DropAreaStyle);
                                        GUI.color = GUIStyles.DefaultBackgroundColor;

                                        Event evt = Event.current;
                                        switch (evt.type)
                                        {
                                            case EventType.DragUpdated:
                                            case EventType.DragPerform:

                                                if (prefabDropArea.Contains(evt.mousePosition))
                                                {

                                                    DragAndDrop.visualMode = DragAndDropVisualMode.Copy;

                                                    if (evt.type == EventType.DragPerform)
                                                    {
                                                        DragAndDrop.AcceptDrag();

                                                        // list of new prefabs that should be created via drag/drop
                                                        // we can't do it in the drag/drop code itself, we'd get exceptions like
                                                        //   ArgumentException: Getting control 12's position in a group with only 12 controls when doing dragPerform. Aborting
                                                        // followed by
                                                        //   Unexpected top level layout group! Missing GUILayout.EndScrollView/EndVertical/EndHorizontal? UnityEngine.GUIUtility:ProcessEvent(Int32, IntPtr)
                                                        // they must be added when everything is done (currently at the end of this method)
                                                        editor.newDraggedPrefabs = new List<PrefabSettings>();

                                                        foreach (Object droppedObject in DragAndDrop.objectReferences)
                                                        {

                                                            // allow only prefabs
                                                            if (PrefabUtility.GetPrefabAssetType(droppedObject) == PrefabAssetType.NotAPrefab)
                                                            {
                                                                Debug.Log("Not a prefab: " + droppedObject);
                                                                continue;
                                                            }

                                                            // add the prefab to the list using the template
                                                            AddPrefab(droppedObject as GameObject, template);

                                                        }
                                                    }
                                                }
                                                break;
                                        }
                                    }
                                
                                }
                            }
                            GUILayout.EndHorizontal();
                        }

                        editor.SetDefaultBackgroundColor();

                    }
                    GUILayout.EndVertical();
                }
                GUILayout.EndHorizontal();

                #endregion template drop targets

                if (editorTarget.prefabSettingsList.Count > 0)
                {
                    EditorGUILayout.Space();
                }

                for (int i = 0; i < editorTarget.prefabSettingsList.Count; i++)
                {
                    if (i > 0)
                        editor.addGUISeparator();

                    PrefabSettings prefabSettings = this.editorTarget.prefabSettingsList[i];

                    GUILayout.BeginHorizontal();
                    {
                        // preview

                        // try to get the asset preview
                        Texture2D previewTexture = AssetPreview.GetAssetPreview(prefabSettings.prefab);

                        // if no asset preview available, try to get the mini thumbnail
                        if (!previewTexture)
                        {
                            previewTexture = AssetPreview.GetMiniThumbnail(prefabSettings.prefab);
                        }

                        // if a preview is available, paint it
                        if (previewTexture)
                        {
                            //GUILayout.Label(previewTexture, EditorStyles.objectFieldThumb, GUILayout.Width(50), GUILayout.Height(50)); // without border, but with size
                            GUILayout.Label(previewTexture, GUILayout.Width(50), GUILayout.Height(50)); // without border, but with size

                            //GUILayout.Box(previewTexture); // with border
                            //GUILayout.Label(previewTexture); // no border
                            //GUILayout.Box(previewTexture, GUILayout.Width(50), GUILayout.Height(50)); // with border and size
                            //EditorGUI.DrawPreviewTexture(new Rect(25, 60, 100, 100), previewTexture); // draws it in absolute coordinates

                        }

                        // right alin the buttons
                        GUILayout.FlexibleSpace();

                        if (GUILayout.Button("Add", EditorStyles.miniButton))
                        {
                            this.editorTarget.prefabSettingsList.Insert(i + 1, new PrefabSettings());
                        }
                        if (GUILayout.Button("Duplicate", EditorStyles.miniButton))
                        {
                            PrefabSettings newPrefabSettings = prefabSettings.Clone();
                            this.editorTarget.prefabSettingsList.Insert(i + 1, newPrefabSettings);
                        }
                        if (GUILayout.Button("Reset", EditorStyles.miniButton))
                        {
                            // remove existing
                            this.editorTarget.prefabSettingsList.RemoveAt(i);

                            // add new
                            this.editorTarget.prefabSettingsList.Insert(i, new PrefabSettings());

                        }
                        if (GUILayout.Button("Remove", EditorStyles.miniButton))
                        {
                            this.editorTarget.prefabSettingsList.Remove(prefabSettings);
                        }

                    }
                    GUILayout.EndHorizontal();

                    GUILayout.Space(4);

                    prefabSettings.prefab = (GameObject)EditorGUILayout.ObjectField("Prefab", prefabSettings.prefab, typeof(GameObject), true);

                    prefabSettings.active = EditorGUILayout.Toggle("Active", prefabSettings.active);
                    prefabSettings.probability = EditorGUILayout.Slider("Probability", prefabSettings.probability, 0, 1);

                    // scale
                    prefabSettings.changeScale = EditorGUILayout.Toggle("Change Scale", prefabSettings.changeScale);

                    if (prefabSettings.changeScale)
                    {
                        prefabSettings.scaleMin = EditorGUILayout.FloatField("Scale Min", prefabSettings.scaleMin);
                        prefabSettings.scaleMax = EditorGUILayout.FloatField("Scale Max", prefabSettings.scaleMax);
                    }

                    // position
                    prefabSettings.positionOffset = EditorGUILayout.Vector3Field("Position Offset", prefabSettings.positionOffset);

                    // rotation
                    prefabSettings.rotationOffset = EditorGUILayout.Vector3Field("Rotation Offset", prefabSettings.rotationOffset);
                    prefabSettings.randomRotation = EditorGUILayout.Toggle("Random Rotation", prefabSettings.randomRotation);

                    // rotation limits
                    if (prefabSettings.randomRotation)
                    {
                        EditorGuiUtilities.MinMaxEditor("  Rotation Limit X", ref prefabSettings.rotationMinX, ref prefabSettings.rotationMaxX, 0, 360);
                        EditorGuiUtilities.MinMaxEditor("  Rotation Limit Y", ref prefabSettings.rotationMinY, ref prefabSettings.rotationMaxY, 0, 360);
                        EditorGuiUtilities.MinMaxEditor("  Rotation Limit Z", ref prefabSettings.rotationMinZ, ref prefabSettings.rotationMaxZ, 0, 360);
                    }

                    // VS Pro Id
#if VEGETATION_STUDIO_PRO
                    EditorGUI.BeginDisabledGroup(true);
                    EditorGUILayout.TextField("Asset GUID", prefabSettings.assetGUID);
                    EditorGUILayout.TextField("VSPro Id", prefabSettings.vspro_VegetationItemID);
                    EditorGUI.EndDisabledGroup();
#endif
                }
            }

            GUILayout.EndVertical();

        }

        public void OnSceneGUI()
        {
        }

        private void AddPrefab(GameObject prefab, PrefabSettingsTemplate template)
        {
            // new settings
            PrefabSettings prefabSettings = new PrefabSettings();

            prefabSettings.ApplyTemplate(template);

            // initialize with dropped prefab
            prefabSettings.prefab = prefab;

            editor.newDraggedPrefabs.Add(prefabSettings);

        }

    }
}
