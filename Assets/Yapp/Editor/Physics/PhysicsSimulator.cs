/**
 * Original version by Jazz Macedo
 * https://gist.github.com/jasielmacedo/c5391fd145572bebbe2b5052e3a38495
 */
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEditor;
using UnityEngine;

namespace Yapp
{
    // This causes the class' static constructor to be called on load and on starting playmode
    [InitializeOnLoad]
    public static class PhysicsSimulator
    {
        // only ever register once
        static bool registered = false;

        // how long do we run physics for before we give up getting things to sleep
        static float timeToSettle = 3f;

        // how long have we been running
        static float activeTime = 0f;

        static bool active = false;

        static List<PhysicsSimulationGroup> groupRegistry = new List<PhysicsSimulationGroup>();
        static List<PhysicsSimulationGroup> undoList = new List<PhysicsSimulationGroup>();

        static int simulationSteps = 1;

        static PhysicsSimulator()
        {
            if (!registered)
            {
                // hook into the editor update
                EditorApplication.update += Update;

                // and the scene view OnGui
                SceneView.duringSceneGui += OnSceneGUI;

                registered = true;
            }
        }

        public static void SetSimulationSteps( int steps)
        {
            simulationSteps = steps;
        }

        public static void SetSimulationTime( float time)
        {
            timeToSettle = time;
        }

        public static void RegisterGroup(PhysicsSimulationGroup group)
        {
            groupRegistry.Add(group);
        }

        public static bool IsActive()
        {
            return active;
        }

        public static void Activate()
        {

            if (!active)
            {
                //Debug.Log("Physics started");

                active = true;

                //// Normally avoid Find functions, but this is editor time and only happens once
                //workList = Object.FindObjectsOfType<Rigidbody>();

                activeTime = 0f;

                //// make sure that all rigidbodies are awake so they will actively settle against changed geometry.
                //foreach (Rigidbody body in workList)
                //{
                //    body.WakeUp();
                //}
            }
            else
            {
                //Debug.Log("Physics restarted");

                // reset
                activeTime = 0f;

            }
        }

        public static void Stop()
        {
            active = false;

            if (groupRegistry.Count > 0)
            {
                undoList.Clear();
                undoList.AddRange(groupRegistry);

                foreach (PhysicsSimulationGroup group in groupRegistry)
                {
                    group.CleanUp();
                }

                groupRegistry.Clear();

                //Debug.Log("Physics stopped");
            }

        }

        private static void Update()
        {
            if (active)
            {

                // save original setting
                bool prevAutoSimulation = Physics.autoSimulation;

                try
                {
                    foreach (PhysicsSimulationGroup group in groupRegistry)
                    {
                        group.PerformSimulateStep();
                    }

                    activeTime += Time.deltaTime;

                    // make sure we are not autosimulating
                    Physics.autoSimulation = false;

                    // see if all our 
                    //bool allSleeping = true;
                    //foreach (Rigidbody body in workList)
                    //{
                    //    if (body != null)
                    //    {
                    //        allSleeping &= body.IsSleeping();
                    //    }
                    //} 

                    if (/*allSleeping ||*/ activeTime >= timeToSettle)
                    {

                        Stop();
                    }
                    else
                    {
                        for (int i = 0; i < simulationSteps; i++)
                        {
                            Physics.Simulate(Time.deltaTime);
                        }
                    }

                } finally {
                    // restore original setting
                    Physics.autoSimulation = prevAutoSimulation;
                }
            }

        }

        static void OnSceneGUI(SceneView sceneView)
        {

            if (active)
            {
                Color prevColor = GUI.color;
                {
                    Handles.BeginGUI();
                    {
                        GUI.color = Color.red;

                        GUILayout.BeginArea(new Rect(10, 10, 100, 100));
                        GUILayout.Label("Physics Active", GUI.skin.box, GUILayout.Width(100));
                        GUILayout.Label(string.Format(CultureInfo.InvariantCulture, "Time Left: {0:F2}", (timeToSettle - activeTime)), GUI.skin.box, GUILayout.Width(100));
                        GUILayout.EndArea();

                    }
                    Handles.EndGUI();
                }
                GUI.color = prevColor;
            }
        }

        public static void UndoSimulation()
        {
            foreach (PhysicsSimulationGroup group in undoList)
            {
                group.UndoSimulation();
            }
        }
    }
}