﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Yapp
{
    [System.Serializable]
    public class PrefabSettings
    {
        /// <summary>
        /// The name which will be displayed in the prefab template grid of the inspector
        /// </summary>
        public string templateName;

        /// <summary>
        /// The prefab which should be instanted and placed at the brush position
        /// </summary>
        [HideInInspector]
        public GameObject prefab;

        /// <summary>
        /// Whether the prefab is used or not
        /// </summary>
        public bool active = true;

        /// <summary>
        /// The probability at which the prefab is chosen to be instantiated.
        /// This value is relative to all other prefabs.
        /// So 0 doesn't mean it won't be instantiated at all, it means it's less probable
        /// to be instantiated than others which don't have 0.
        /// Ranges from 0 (not probable at all) to 1 (highest probability).
        /// The value is relative. If all a
        /// </summary>
        public float probability = 1;

        /// <summary>
        /// The offset that should be added to the instantiated gameobjects position.
        /// This is useful to correct the position of prefabs. 
        /// It's also useful in combination with the physics module in order to let e. g. pumpkins fall naturally on the terrain.
        /// </summary>
        public Vector3 positionOffset;

        /// <summary>
        /// The offset that should be added to the instantiated gameobjects rotation.
        /// This is useful to correct the rotation of prefabs.
        /// The offset is Vector3, uses Eulers.
        /// </summary>
        public Vector3 rotationOffset;

        /// <summary>
        /// Randomize rotation
        /// </summary>
        public bool randomRotation;

        /// <summary>
        /// Minimum X rotation in degrees when random rotation is used.
        /// </summary>
        public float rotationMinX = 0f;

        /// <summary>
        /// Maximum X rotation in degrees when random rotation is used.
        /// </summary>
        public float rotationMaxX = 360f;

        /// <summary>
        /// Minimum Y rotation in degrees when random rotation is used.
        /// </summary>
        public float rotationMinY = 0f;

        /// <summary>
        /// Maximum Y rotation in degrees when random rotation is used.
        /// </summary>
        public float rotationMaxY = 360f;

        /// <summary>
        /// Minimum Z rotation in degrees when random rotation is used.
        /// </summary>
        public float rotationMinZ = 0f;

        /// <summary>
        /// Maximum Z rotation in degrees when random rotation is used.
        /// </summary>
        public float rotationMaxZ = 360f;

        /// <summary>
        /// Randomize Scale Minimum
        /// </summary>
        public bool changeScale = false;

        /// <summary>
        /// Randomize Scale Minimum
        /// </summary>
        public float scaleMin = 0.5f;

        /// <summary>
        /// Randomize Scale Maximum
        /// </summary>
        public float scaleMax = 1.5f;

        /// <summary>
        /// Storing asset GUID here for future reference (performance reasons)
        /// </summary>
        [HideInInspector]
        public string assetGUID = null;

        /// <summary>
        /// Vegetation Studio Pro vspro_VegetationItemID
        /// </summary>
        [HideInInspector]
        public string vspro_VegetationItemID = null;

        /// <summary>
        /// Apply the settings of the template to the current prefab settings
        /// </summary>
        /// <param name="template"></param>
        public void ApplyTemplate(PrefabSettingsTemplate template)
        {

            active = template.active;
            probability = template.probability;
            positionOffset = template.positionOffset;
            rotationOffset = template.rotationOffset;
            randomRotation = template.randomRotation;
            rotationMinX = template.rotationMinX;
            rotationMaxX = template.rotationMaxX;
            rotationMinY = template.rotationMinY;
            rotationMaxY = template.rotationMaxY;
            rotationMinZ = template.rotationMinZ;
            rotationMaxZ = template.rotationMaxZ;
            changeScale = template.changeScale;
            scaleMin = template.scaleMin;
            scaleMax = template.scaleMax;

        }

        public PrefabSettings Clone()
        {
            return (PrefabSettings)this.MemberwiseClone();
        }
    }
}
