using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Yapp
{
    [System.Serializable]
    public class ControlPoint
    {
        [SerializeField]
        public Vector3 position = Vector3.zero;

        [SerializeField]
        public Quaternion rotation = Quaternion.identity;

        /// <summary>
        /// Override ToString() for better debug info
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "ControlPoint = [ position = " + position + ", rotation = " + rotation + "]";
        }
    }

}
