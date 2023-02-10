using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Yapp
{
    [System.Serializable]
    public class PhysicsSettings
    {
        public enum ForceApplyType
        {
            /// <summary>
            /// Apply the force at the start of a simulation.
            /// Like an explosion when you have a lot of prefabs at the same location.
            /// </summary>
            Initial,

            /// <summary>
            /// Apply the force continuously during the simulation.
            /// Like wind blowing the prefabs away
            /// </summary>
            Continuous
        }

        #region Public Editor Fields

        public ForceApplyType forceApplyType = ForceApplyType.Initial;
        public int maxIterations = 1000;
        public Vector2 forceMinMax = Vector2.zero;
        public float forceAngleInDegrees = 0f;
        public bool randomizeForceAngle = false;

        #endregion Public Editor Fields

        /// <summary>
        /// The time in seconds for which the physics simulator should run
        /// </summary>
        [Range(1,60)]
        public float simulationTime = 3f;

        /// <summary>
        /// The number of physics iterations iterations that should run per frame
        /// </summary>
        [Range(1,1000)]
        public int simulationSteps = 1;
    }
}
