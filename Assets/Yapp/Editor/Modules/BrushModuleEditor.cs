using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using static Yapp.BrushComponent;


namespace Yapp
{
    public class BrushModuleEditor: ModuleEditorI
    {
        #region Properties

        SerializedProperty brushSize;
        SerializedProperty brushRotation;
        SerializedProperty allowOverlap;
        SerializedProperty alignToTerrain;
        SerializedProperty distribution;
        SerializedProperty poissonDiscSize;
        SerializedProperty poissonDiscRaycastOffset;
        SerializedProperty fallOffCurve;
        SerializedProperty fallOff2dCurveX;
        SerializedProperty fallOff2dCurveZ;
        SerializedProperty curveSamplePoints;

        #endregion Properties

        #region Integration to external applications
        VegetationStudioProIntegration vegetationStudioProIntegration;
        #endregion Integration to external applications

        #pragma warning disable 0414
        PrefabPainterEditor editor;
        #pragma warning restore 0414
         
        PrefabPainter editorTarget;

        BrushComponent brushComponent = new BrushComponent();

        /// <summary>
        /// Auto physics only on special condition:
        /// + prefabs were added
        /// + mouse got released
        /// </summary>
        private bool needsPhysicsApplied = false;

        private List<GameObject> autoPhysicsCollection = new List<GameObject>();

        public BrushModuleEditor(PrefabPainterEditor editor)
        {
            this.editor = editor;
            this.editorTarget = editor.GetPainter();


            brushSize = editor.FindProperty( x => x.brushSettings.brushSize);
            brushRotation = editor.FindProperty(x => x.brushSettings.brushRotation);

            alignToTerrain = editor.FindProperty(x => x.brushSettings.alignToTerrain);
            distribution = editor.FindProperty(x => x.brushSettings.distribution);
            poissonDiscSize = editor.FindProperty(x => x.brushSettings.poissonDiscSize);
            poissonDiscRaycastOffset = editor.FindProperty(x => x.brushSettings.poissonDiscRaycastOffset);
            fallOffCurve = editor.FindProperty(x => x.brushSettings.fallOffCurve);
            fallOff2dCurveX = editor.FindProperty(x => x.brushSettings.fallOff2dCurveX);
            fallOff2dCurveZ = editor.FindProperty(x => x.brushSettings.fallOff2dCurveZ);
            curveSamplePoints = editor.FindProperty(x => x.brushSettings.curveSamplePoints);
            allowOverlap = editor.FindProperty(x => x.brushSettings.allowOverlap);

            // initialize integrated applications
            vegetationStudioProIntegration = new VegetationStudioProIntegration( editor);

        }

        public void OnInspectorGUI()
        {
            GUILayout.BeginVertical("box");

            EditorGUILayout.LabelField("Brush settings", GUIStyles.BoxTitleStyle);

            EditorGUILayout.PropertyField(brushSize, new GUIContent("Brush Size"));
            EditorGUILayout.PropertyField(brushRotation, new GUIContent("Brush Rotation"));

            EditorGUILayout.PropertyField(alignToTerrain, new GUIContent("Align To Terrain"));
            EditorGUILayout.PropertyField(allowOverlap, new GUIContent("Allow Overlap", "Center Mode: Check against brush size.\nPoisson Mode: Check against Poisson Disc size"));

            EditorGUILayout.PropertyField(distribution, new GUIContent("Distribution"));

            switch (editorTarget.brushSettings.distribution)
            {
                case BrushSettings.Distribution.Center:
                    break;
                case BrushSettings.Distribution.Poisson_Any:
                    EditorGUI.indentLevel++;
                    EditorGUILayout.PropertyField(poissonDiscSize, new GUIContent("Poisson Disc Size"));
                    EditorGUILayout.PropertyField(poissonDiscRaycastOffset, new GUIContent("Raycast Offset", "If any collider (not only terrain) is used for the raycast, then this will used as offset from which the ray will be cast against the collider"));
                    EditorGUI.indentLevel--;
                    break;
                case BrushSettings.Distribution.Poisson_Terrain:
                    EditorGUI.indentLevel++;
                    EditorGUILayout.PropertyField(poissonDiscSize, new GUIContent("Poisson Disc Size"));
                    EditorGUI.indentLevel--;
                    break;
                case BrushSettings.Distribution.FallOff:
                    EditorGUILayout.PropertyField(curveSamplePoints, new GUIContent("Curve Sample Points"));
                    EditorGUILayout.PropertyField(fallOffCurve, new GUIContent("FallOff"));
                    break;
                case BrushSettings.Distribution.FallOff2d:
                    EditorGUILayout.PropertyField(curveSamplePoints, new GUIContent("Curve Sample Points"));
                    EditorGUILayout.PropertyField(fallOff2dCurveX, new GUIContent("FallOff X"));
                    EditorGUILayout.PropertyField(fallOff2dCurveZ, new GUIContent("FallOff Z"));
                    break;
            }

            // TODO: how to create a minmaxslider with propertyfield?
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("Slope");
            EditorGUILayout.MinMaxSlider(ref editorTarget.brushSettings.slopeMin, ref editorTarget.brushSettings.slopeMax, editorTarget.brushSettings.slopeMinLimit, editorTarget.brushSettings.slopeMaxLimit);
            EditorGUILayout.EndHorizontal();

            // vegetation studio pro
            vegetationStudioProIntegration.OnInspectorGUI();

            // consistency check
            float minDiscSize = 0.01f;
            if( poissonDiscSize.floatValue < minDiscSize)
            {
                Debug.LogError("Poisson Disc Size is too small. Setting it to " + minDiscSize);
                poissonDiscSize.floatValue = minDiscSize;
            }

            GUILayout.EndVertical();

        }



        public void OnSceneGUI()
        {

            // paint prefabs on mouse drag. don't do anything if no mode is selected, otherwise e.g. movement in scene view wouldn't work with alt key pressed
            if ( brushComponent.DrawBrush(editorTarget.brushSettings, out BrushMode brushMode, out RaycastHit raycastHit))
            {
                switch( brushMode)
                {
                    case BrushMode.ShiftDrag:

                        AddPrefabs(raycastHit);

                        needsPhysicsApplied = true;

                        // consume event
                        Event.current.Use();
                        break;

                    case BrushMode.ShiftCtrlDrag:

                        RemovePrefabs(raycastHit);

                        // consume event
                        Event.current.Use();
                        break;

                }
            }

            // info for the scene gui; used to be dynamic and showing number of prefabs (currently is static until refactoring is done)
            string[] guiInfo = new string[] { "Add prefabs: shift + drag mouse\nRemove prefabs: shift + ctrl + drag mouse\nBrush size: ctrl + mousewheel, Brush rotation: ctrl + shift + mousewheel" };
            brushComponent.Layout(guiInfo);

            // auto physics
            bool applyAutoPhysics = needsPhysicsApplied 
                && autoPhysicsCollection.Count > 0
                && editorTarget.spawnSettings.autoSimulationType != SpawnSettings.AutoSimulationType.None 
                && Event.current.type == EventType.MouseUp;
            if (applyAutoPhysics)
            {
                AutoPhysicsSimulation.ApplyPhysics(editorTarget.physicsSettings, autoPhysicsCollection, editorTarget.spawnSettings.autoSimulationType);
                
                autoPhysicsCollection.Clear();
            }

        }
        


        #region Paint Prefabs

        private void AddPrefabs(RaycastHit hit)
        {
            if (!editor.IsEditorSettingsValid())
                return;

            switch (editorTarget.brushSettings.distribution)
            {
                case BrushSettings.Distribution.Center:
                    AddPrefabs_Center(hit.point, hit.normal);
                    break;
                case BrushSettings.Distribution.Poisson_Any:
                    AddPrefabs_Poisson_Any(hit.point, hit.normal);
                    break;
                case BrushSettings.Distribution.Poisson_Terrain:
                    AddPrefabs_Poisson_Terrain(hit.point, hit.normal);
                    break;
                case BrushSettings.Distribution.FallOff:
                    Debug.Log("Not implemented yet: " + editorTarget.brushSettings.distribution);
                    break;
                case BrushSettings.Distribution.FallOff2d:
                    Debug.Log("Not implemented yet: " + editorTarget.brushSettings.distribution);
                    break;
            }

        }

        /// <summary>
        /// Add prefabs, mode Center
        /// </summary>
        private void AddPrefabs_Center( Vector3 position, Vector3 normal)
        {

            // check if a gameobject is already within the brush size
            // allow only 1 instance per bush size
            GameObject container = editorTarget.container as GameObject;


            // check if a prefab already exists within the brush
            bool prefabExists = false;

            // check overlap
            if (!editorTarget.brushSettings.allowOverlap)
            {
                float brushRadius = editorTarget.brushSettings.brushSize / 2f;

                foreach (Transform child in container.transform)
                {
                    float dist = Vector3.Distance(position, child.transform.position);

                    // check against the brush
                    if (dist <= brushRadius)
                    {
                        prefabExists = true;
                        break;
                    }

                }
            }

            if (!prefabExists)
            {
                AddNewPrefab(position, normal);
            }
        }



        private void AddNewPrefab( Vector3 position, Vector3 normal)
        {

            GameObject container = editorTarget.container as GameObject;

            PrefabSettings prefabSettings = this.editorTarget.GetPrefabSettings();

            GameObject prefab = prefabSettings.prefab;

            ///
            /// Calculate position / rotation / scale
            /// 

            // get new position
            Vector3 newPosition = position;

            // add offset
            newPosition += prefabSettings.positionOffset;

            // auto physics height offset
            newPosition = ApplyAutoPhysicsHeightOffset(newPosition);

            Vector3 newLocalScale = prefabSettings.prefab.transform.localScale;

            // size
            if (prefabSettings.changeScale)
            {
                newLocalScale = Vector3.one * Random.Range(prefabSettings.scaleMin, prefabSettings.scaleMax);
            }

            // rotation
            Quaternion alignedRotation = Quaternion.identity;
            Quaternion objectRotation;

            if (this.editorTarget.brushSettings.alignToTerrain)
            {
                alignedRotation = Quaternion.FromToRotation(Vector3.up, normal);
            }

            if (prefabSettings.randomRotation)
            {

                float rotationX = Random.Range(prefabSettings.rotationMinX, prefabSettings.rotationMaxX);
                float rotationY = Random.Range(prefabSettings.rotationMinY, prefabSettings.rotationMaxY);
                float rotationZ = Random.Range(prefabSettings.rotationMinZ, prefabSettings.rotationMaxZ);

                objectRotation = Quaternion.Euler(rotationX, rotationY, rotationZ);
            }
            else
            {
                objectRotation = Quaternion.Euler(prefabSettings.rotationOffset);
            }

            // combine terrain aligned rotation and object rotation
            Quaternion newRotation = alignedRotation * objectRotation;


            ///
            /// create instance and apply position / rotation / scale
            /// 

            // spawn item to vs pro
            if ( editorTarget.brushSettings.spawnToVSPro)
            {
                vegetationStudioProIntegration.AddNewPrefab(prefabSettings, newPosition, newRotation, newLocalScale);
            }
            // spawn item to scene
            else
            {

                // new prefab
                GameObject instance = PrefabUtility.InstantiatePrefab(prefab) as GameObject;

                instance.transform.position = newPosition;
                instance.transform.rotation = newRotation;
                instance.transform.localScale = newLocalScale;

                // attach as child of container
                instance.transform.parent = container.transform;

                Undo.RegisterCreatedObjectUndo(instance, "Instantiate Prefab");

                if (editorTarget.spawnSettings.autoSimulationType != SpawnSettings.AutoSimulationType.None)
                {
                    autoPhysicsCollection.Add(instance);
                }
            }

        }

        /// <summary>
        /// Add prefabs, mode Poisson
        /// </summary>
        private void AddPrefabs_Poisson_Any(Vector3 position, Vector3 normal)
        {
            GameObject container = editorTarget.container as GameObject;

            float brushSize = editorTarget.brushSettings.brushSize;
            float brushRadius = brushSize / 2.0f;
            float discRadius = editorTarget.brushSettings.poissonDiscSize / 2;

            PoissonDiscSampler sampler = new PoissonDiscSampler(brushSize, brushSize, discRadius);

            foreach (Vector2 sample in sampler.Samples()) {

                // brush is currenlty a disc => ensure the samples are within the disc
                if (Vector2.Distance(sample, new Vector2(brushRadius, brushRadius)) > brushRadius)
                    continue;

                // x/z come from the poisson sample 
                float x = position.x + sample.x - brushRadius;
                float z = position.z + sample.y - brushRadius;

                float y = position.y + editorTarget.brushSettings.poissonDiscRaycastOffset;
                Vector3 currentPosition = new Vector3(x, y, z);

				// TODO: raycast hit against layer
				//       see https://docs.unity3d.com/ScriptReference/Physics.Raycast.html
				if (Physics.Raycast(currentPosition, Vector3.down, out RaycastHit raycastHitDown, Mathf.Infinity))
				{
					y = raycastHitDown.point.y;
				}
				else if (Physics.Raycast(currentPosition, Vector3.up, out RaycastHit raycastHitUp, Mathf.Infinity))
				{
					y = raycastHitUp.point.y;
				}
				else
				{
					continue;
				}

				// create position vector
				Vector3 prefabPosition = new Vector3( x, y, z);

                // auto physics height offset
                prefabPosition = ApplyAutoPhysicsHeightOffset(prefabPosition);

                // check if a prefab already exists within the brush
                bool prefabExists = false;

                // check overlap
                if (!editorTarget.brushSettings.allowOverlap)
                {
                    foreach (Transform child in container.transform)
                    {
                        float dist = Vector3.Distance(prefabPosition, child.transform.position);

                        // check against a single poisson disc
                        if (dist <= discRadius)
                        {
                            prefabExists = true;
                            break;
                        }

                    }
                }

                // add prefab
                if( !prefabExists)
                {
                    AddNewPrefab(prefabPosition, normal);
                }
            }
        }

        /// <summary>
        /// Add prefabs, mode Poisson
        /// </summary>
        private void AddPrefabs_Poisson_Terrain(Vector3 position, Vector3 normal)
        {
            if (!Terrain.activeTerrain)
                return;

            GameObject container = editorTarget.container as GameObject;

            float brushSize = editorTarget.brushSettings.brushSize;
            float brushRadius = brushSize / 2.0f;
            float discRadius = editorTarget.brushSettings.poissonDiscSize / 2;

            PoissonDiscSampler sampler = new PoissonDiscSampler(brushSize, brushSize, discRadius);

            foreach (Vector2 sample in sampler.Samples())
            {

                // brush is currenlty a disc => ensure the samples are within the disc
                if (Vector2.Distance(sample, new Vector2(brushRadius, brushRadius)) > brushRadius)
                    continue;

                // x/z come from the poisson sample 
                float x = position.x + sample.x - brushRadius;
                float z = position.z + sample.y - brushRadius;

                // y depends on the terrain height
                Vector3 terrainPosition = new Vector3(x, position.y, z);

                // get terrain y position and add Terrain Transform Y-Position
                float y = Terrain.activeTerrain.SampleHeight(terrainPosition) + Terrain.activeTerrain.GetPosition().y;

                // create position vector
                Vector3 prefabPosition = new Vector3(x, y, z);

                // auto physics height offset
                prefabPosition = ApplyAutoPhysicsHeightOffset(prefabPosition);

                // check if a prefab already exists within the brush
                bool prefabExists = false;

                // check overlap
                if (!editorTarget.brushSettings.allowOverlap)
                {
                    foreach (Transform child in container.transform)
                    {
                        float dist = Vector3.Distance(prefabPosition, child.transform.position);

                        // check against a single poisson disc
                        if (dist <= discRadius)
                        {
                            prefabExists = true;
                            break;
                        }

                    }
                }

                // add prefab
                if (!prefabExists)
                {
                    AddNewPrefab(prefabPosition, normal);
                }
            }
        }
        

        /// <summary>
        /// Add additional height offset if auto physics is enabled
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        private Vector3 ApplyAutoPhysicsHeightOffset( Vector3 position)
        {
            if (editorTarget.spawnSettings.autoSimulationType == SpawnSettings.AutoSimulationType.None)
                return position;

            // auto physics: add additional height offset
            position.y += editorTarget.spawnSettings.autoSimulationHeightOffset;

            return position;
        }

        /// <summary>
        /// Remove prefabs
        /// </summary>
        private void RemovePrefabs( RaycastHit raycastHit)
        {

            if (!editor.IsEditorSettingsValid())
                return;

            Vector3 position = raycastHit.point;

            // check if a gameobject of the container is within the brush size and remove it
            GameObject container = editorTarget.container as GameObject;

            float radius = editorTarget.brushSettings.brushSize / 2f;

            List<Transform> removeList = new List<Transform>();

            foreach (Transform transform in container.transform)
            {
                float dist = Vector3.Distance(position, transform.transform.position);

                if (dist <= radius)
                {
                    removeList.Add(transform);
                }

            }

            // remove gameobjects
            foreach( Transform transform in removeList)
            {
                PrefabPainter.DestroyImmediate(transform.gameObject);
            }
           
        }

        #endregion Paint Prefabs


    }

}
