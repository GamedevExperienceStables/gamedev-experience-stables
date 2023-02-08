using UnityEngine;
using UnityEditor;
using static Yapp.BrushComponent;

namespace Yapp
{
    public class InteractionModuleEditor: ModuleEditorI
    {
        #region Properties

        SerializedProperty interactionType;
        SerializedProperty antiGravityStrength;
        SerializedProperty magnetStrength;

        #endregion Properties

#pragma warning disable 0414
        PrefabPainterEditor editor;
        PrefabPainter editorTarget;
#pragma warning restore 0414

        BrushComponent brushComponent = new BrushComponent();

        /// <summary>
        /// Auto physics only on special condition:
        /// + prefabs were added
        /// + mouse got released
        /// </summary>
        private bool needsPhysicsApplied = false; // TODO property


        public InteractionModuleEditor(PrefabPainterEditor editor)
        {
            this.editor = editor;
            this.editorTarget = editor.GetPainter();

            interactionType = editor.FindProperty(x => x.interactionSettings.interactionType);
            antiGravityStrength = editor.FindProperty(x => x.interactionSettings.antiGravityStrength);
            magnetStrength = editor.FindProperty(x => x.interactionSettings.magnetStrength);

        }

        public void OnInspectorGUI()
        {

            GUILayout.BeginVertical("box");

            EditorGUILayout.LabelField("Interaction (Experimental)", GUIStyles.BoxTitleStyle);

            EditorGUILayout.HelpBox("Perform interactive operations on the container children\nThis is highly experimental and bound to change", MessageType.Info);

            GUILayout.EndVertical();


            GUILayout.BeginVertical("box");

            EditorGUILayout.LabelField("Interaction Type", GUIStyles.BoxTitleStyle);

            EditorGUILayout.PropertyField(interactionType, new GUIContent("Type", "Type of interaction"));

            GUILayout.EndVertical();


            if (interactionType.enumValueIndex == (int) InteractionSettings.InteractionType.AntiGravity)
            {

                GUILayout.BeginVertical("box");

                EditorGUILayout.LabelField("Anti-Gravity", GUIStyles.BoxTitleStyle);

                EditorGUILayout.PropertyField(antiGravityStrength, new GUIContent("Strength", "Increments in Y-Position per editor step"));

                GUILayout.EndVertical();
            }


            if (interactionType.enumValueIndex == (int)InteractionSettings.InteractionType.Magnet)
            {
                GUILayout.BeginVertical("box");

                EditorGUILayout.LabelField("Magnet", GUIStyles.BoxTitleStyle);

                EditorGUILayout.PropertyField(magnetStrength, new GUIContent("Strength", "Strength of the Magnet"));

                GUILayout.EndVertical();
            }

        }

        public void OnSceneGUI()
        {

            // paint prefabs on mouse drag. don't do anything if no mode is selected, otherwise e.g. movement in scene view wouldn't work with alt key pressed
            if (brushComponent.DrawBrush(editorTarget.brushSettings, out BrushMode brushMode, out RaycastHit raycastHit))
            {
                if( editorTarget.interactionSettings.interactionType == InteractionSettings.InteractionType.AntiGravity)
                {
                    switch (brushMode)
                    {
                        case BrushMode.ShiftPressed:

                            AntiGravity(raycastHit);

                            needsPhysicsApplied = true;

                            // don't consume event; mustn't be consumed during layout or repaint
                            //Event.current.Use();
                            break;
                    }

                 }

                if (editorTarget.interactionSettings.interactionType == InteractionSettings.InteractionType.Magnet)
                {

                    switch (brushMode)
                    {
                        case BrushMode.ShiftPressed:

                            Attract(raycastHit);

                            needsPhysicsApplied = true;

                            // don't consume event; mustn't be consumed during layout or repaint
                            //Event.current.Use();
                            break;

                        case BrushMode.ShiftCtrlPressed:

                            Repell(raycastHit);

                            needsPhysicsApplied = true;

                            // don't consume event; mustn't be consumed during layout or repaint
                            //Event.current.Use();
                            break;

                    }

                }

            }

            // TODO: change text
            // info for the scene gui; used to be dynamic and showing number of prefabs (currently is static until refactoring is done)
            string[] guiInfo = new string[] { "Add prefabs: shift + drag mouse\nRemove prefabs: shift + ctrl + drag mouse\nBrush size: ctrl + mousewheel, Brush rotation: ctrl + shift + mousewheel" };
            brushComponent.Layout(guiInfo);

            // auto physics
            bool applyAutoPhysics = needsPhysicsApplied && editorTarget.spawnSettings.autoSimulationType != SpawnSettings.AutoSimulationType.None && Event.current.type == EventType.MouseUp;
            if (applyAutoPhysics)
            {
                AutoPhysicsSimulation.ApplyPhysics(editorTarget.physicsSettings, editorTarget.container, editorTarget.spawnSettings.autoSimulationType);
            }
        }


        /// <summary>
        /// Increment y-position in world space
        /// </summary>
        /// <param name="hit"></param>
        private void AntiGravity(RaycastHit hit)
        {
            // just some arbitrary value depending on the magnet strength which ranges from 0..100
            float antiGravityFactor = editorTarget.interactionSettings.antiGravityStrength / 1000f;

            Transform[] containerChildren = PrefabUtils.GetContainerChildren(editorTarget.container);

            foreach (Transform transform in containerChildren)
            {
                Vector3 distance = hit.point - transform.position;

                // only those within the brush
                if (distance.magnitude > editorTarget.brushSettings.brushSize / 2f)
                    continue;

                // https://docs.unity3d.com/ScriptReference/Transform-up.html
                // https://docs.unity3d.com/ScriptReference/Vector3-up.html
                transform.position += Vector3.up * antiGravityFactor;
            }
        }

        private void Attract( RaycastHit hit)
        {
            Magnet(hit, true);
        }

        private void Repell(RaycastHit hit)
        {
            Magnet(hit, false);
        }

        /// <summary>
        /// Attract/Repell the gameobjects of the container which are within the brush
        /// </summary>
        /// <param name="hit"></param>
        /// <param name="attract"></param>
        private void Magnet( RaycastHit hit, bool attract)
        {
            // just some arbitrary value depending on the magnet strength which ranges from 0..100
            float magnetFactor = editorTarget.interactionSettings.magnetStrength / 1000f;

            Transform[] containerChildren = PrefabUtils.GetContainerChildren(editorTarget.container);

            foreach (Transform transform in containerChildren)
            {
                Vector3 distance = hit.point - transform.position;

                // only those within the brush
                if (distance.magnitude > editorTarget.brushSettings.brushSize /2f)
                    continue;

                Vector3 direction = distance.normalized;

                transform.position += direction * magnetFactor * (attract ? 1 : -1);
            }
        }
    }
}
