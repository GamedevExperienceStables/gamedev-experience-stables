using System;
using UnityEngine;

namespace Game.Settings
{
    [Serializable]
    public class CameraSettings
    {
        [SerializeField]
        private float defaultDistance = 5f;

        [SerializeField]
        private float farDistance = 20f;

        [SerializeField]
        private float closeDistance = 5f;

        public float DefaultDistance => defaultDistance;
        public float FarDistance => farDistance;
        public float CloseDistance => closeDistance;
    }
}