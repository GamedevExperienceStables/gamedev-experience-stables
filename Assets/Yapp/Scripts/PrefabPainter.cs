﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Yapp
{
    /// <summary>
    /// Prefab painter
    /// 
    /// Notes:
    /// [ExecuteInEditMode] has become necessary because of the usage of splineModule in OnDrawGizmos
    /// </summary>
    [ExecuteInEditMode]
    public class PrefabPainter : MonoBehaviour
    {

#if UNITY_EDITOR

        public enum Mode { Brush, Spline, Interaction, Container }

        /// <summary>
        /// The parent of the instantiated prefabs 
        /// </summary>
        [HideInInspector] 
        public GameObject container;

        [HideInInspector]
        public Mode mode;

        /// <summary>
        /// The diameter of the brush
        /// </summary>
        [HideInInspector]
        public BrushSettings brushSettings = new BrushSettings();

        /// <summary>
        /// The diameter of the brush
        /// </summary>
        [HideInInspector]
        public SpawnSettings spawnSettings = new SpawnSettings();

        /// <summary>
        /// The prefab that will be instantiated
        /// </summary>
        [HideInInspector]
        public List<PrefabSettings> prefabSettingsList = new List<PrefabSettings>();

        /// <summary>
        /// Instance of PhysicsSimulation.
        /// Keeping the object here allows us to navigate away from the PrefabPainter gameobject
        /// and return to it and keep the phyics settings. Otherwise the physics settings would always be reset
        /// </summary>
        [HideInInspector]
        public PhysicsSettings physicsSettings = new PhysicsSettings();

        /// <summary>
        /// Container for copied positions and rotations
        /// </summary>
        [HideInInspector]
        public Dictionary<int, Geometry> copyPasteGeometryMap = new Dictionary<int, Geometry>();

        /// <summary>
        /// Settings of the spline curve
        /// </summary>
        [HideInInspector]
        public SplineSettings splineSettings = new SplineSettings();

        /// <summary>
        /// Spline module
        /// </summary>
        public SplineModule splineModule = null;

        /// <summary>
        /// Interaction
        /// </summary>
        [HideInInspector]
        public InteractionSettings interactionSettings = new InteractionSettings();

        /// <summary>
        /// This function is called when the script is loaded or a value is changed in the Inspector (Called in the editor only).
        /// You can use this to ensure that when you modify data in an editor, that data stays within a certain range.
        /// https://docs.unity3d.com/ScriptReference/MonoBehaviour.OnValidate.html
        /// </summary>
        void OnValidate()
        {
            if (splineModule == null)
            {
                splineModule = new SplineModule(this);
            }

        }

        void OnDrawGizmosSelected()
        {

            splineModule.OnDrawGizmos();

        }

        /// <summary>
        /// Get a random active prefab setting from the prefab settings list, depending on the probability.
        /// </summary>
        /// <returns></returns>
        public PrefabSettings GetRandomWeightedPrefab()
        {

            if (prefabSettingsList.Count == 0)
                return null;

            float weight;
            float totalSum = 0;

            foreach (var item in prefabSettingsList)
            {
                if (!item.active)
                    continue;

                totalSum += item.probability;

            }

            float random = Random.value;
            float bound = 0f;

            foreach (var item in prefabSettingsList)
            {
                if (!item.active)
                    continue;

                weight = item.probability;

                if( weight <= 0f)
                    continue;

                bound += weight / totalSum;

                if (bound >= random)
                    return item;
            }

            return null;
        }

        public PrefabSettings GetPrefabSettings()
        {

            PrefabSettings selectedItem = GetRandomWeightedPrefab();

            if ( selectedItem == null)
            {
                Debug.LogError("No prefab is active! At least 1 prefab must be active.");
                return null;
            }

            return selectedItem;

        }
#endif
    }
}