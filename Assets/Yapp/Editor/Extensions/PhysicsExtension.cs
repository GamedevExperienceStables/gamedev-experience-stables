using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

namespace Yapp
{

    public class PhysicsExtension 
    {
        #region Properties
        SerializedProperty forceApplyType;
        SerializedProperty maxIterations;
        SerializedProperty forceMinMax;
        SerializedProperty forceAngleInDegrees;
        SerializedProperty randomizeForceAngle;
        SerializedProperty simulationTime;
        SerializedProperty simulationSteps;

        #endregion Properties

#pragma warning disable 0414
        PrefabPainterEditor editor;
        #pragma warning restore 0414

        PrefabPainter editorTarget;

        public PhysicsExtension(PrefabPainterEditor editor)
        {
            this.editor = editor;
            this.editorTarget = editor.GetPainter();

            forceApplyType = editor.FindProperty(x => x.physicsSettings.forceApplyType);
            maxIterations = editor.FindProperty(x => x.physicsSettings.maxIterations);
            forceMinMax = editor.FindProperty(x => x.physicsSettings.forceMinMax);
            forceAngleInDegrees = editor.FindProperty(x => x.physicsSettings.forceAngleInDegrees);
            randomizeForceAngle = editor.FindProperty(x => x.physicsSettings.randomizeForceAngle);
            simulationTime = editor.FindProperty(x => x.physicsSettings.simulationTime);
            simulationSteps = editor.FindProperty(x => x.physicsSettings.simulationSteps);

        }

        public void OnInspectorGUI()
        {
            // separator
            GUILayout.BeginVertical("box");
            //addGUISeparator();

            EditorGUILayout.LabelField("Physics Settings", GUIStyles.BoxTitleStyle);

            #region Settings

            EditorGUILayout.PropertyField(forceApplyType, new GUIContent("Force Apply Type"));

            EditorGUILayout.PropertyField(forceMinMax, new GUIContent("Force Min/Max"));
            EditorGUILayout.PropertyField(forceAngleInDegrees, new GUIContent("Force Angle (Degrees)"));
            EditorGUILayout.PropertyField(randomizeForceAngle, new GUIContent("Randomize Force Angle"));

            #endregion Settings

            EditorGUILayout.Space();

            #region Simulate Continuously

            EditorGUILayout.LabelField("Simulation", GUIStyles.GroupTitleStyle);

            EditorGUILayout.PropertyField(simulationTime, new GUIContent("Time", "The time in seconds for which the physics simulation will be running"));
            EditorGUILayout.PropertyField(simulationSteps, new GUIContent("Steps", "The number of Physics.Simulate() invocations per frame"));

            GUILayout.BeginHorizontal();

            // colorize the button differently in case the physics is running, so that the user gets an indicator that the physics have to be stopped
            // GUI.color = PhysicsSimulator.IsActive() ? GUIStyles.PhysicsRunningButtonBackgroundColor : GUIStyles.DefaultBackgroundColor;
            if (GUILayout.Button("Start"))
            {
                StartSimulation();
            }
            // GUI.color = GUIStyles.DefaultBackgroundColor;

            if (GUILayout.Button("Stop"))
            {
                StopSimulation();
            }

            GUILayout.EndHorizontal();

            #endregion Simulate Continuously

            EditorGUILayout.Space();

            #region Undo
            EditorGUILayout.LabelField("Undo", GUIStyles.GroupTitleStyle);

            if (GUILayout.Button("Undo Last Simulation"))
            {
                ResetAllBodies();
            }
            #endregion Undo

            GUILayout.EndVertical();
        }

        #region Physics Simulation

        private void ResetAllBodies()
        {
            PhysicsSimulator.UndoSimulation();
        }

        #endregion Physics Simulation

        private void StartSimulation()
        {
            Transform[] containerChildren = PrefabUtils.GetContainerChildren(editorTarget.container);
            AutoPhysicsSimulation.ApplyPhysics(editorTarget.physicsSettings, containerChildren, SpawnSettings.AutoSimulationType.Continuous);
        }

        private void StopSimulation()
        {
            PhysicsSimulator.Stop();
        } 

    }
}
