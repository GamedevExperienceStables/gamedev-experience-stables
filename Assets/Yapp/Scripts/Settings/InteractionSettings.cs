using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Yapp
{
    [System.Serializable]
    public class InteractionSettings
    {

        public enum InteractionType
        {
            AntiGravity,
            Magnet,
        }

        #region Public Editor Fields

        public InteractionType interactionType;

        /// <summary>
        /// Anti Gravity strength from 0..100
        /// </summary>
        [Range(0, 100)]
        public int antiGravityStrength = 30;

        /// <summary>
        /// Some arbitrary magnet strength from 0..100
        /// </summary>
        [Range(0,100)]
        public int magnetStrength = 10;

        #endregion Public Editor Fields



    }
}
