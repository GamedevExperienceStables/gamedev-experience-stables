using UnityEngine;

namespace Yapp
{
    [System.Serializable]
    public class SpawnSettings
    {
        public enum AutoSimulationType
        {
            None,
            Continuous
        }

        /// <summary>
        /// Automatically apply physics after painting
        /// </summary>
        public AutoSimulationType autoSimulationType = AutoSimulationType.None;

        /// <summary>
        /// When auto physics is enabled, then this value will be added to the y-position of the gameobject.
        /// This way e. g. rocks are placed higher by default and gravity can be applied
        /// </summary>
        public float autoSimulationHeightOffset = 1f;

    }
}