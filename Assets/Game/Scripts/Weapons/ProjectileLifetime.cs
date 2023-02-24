using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Weapons
{
    [Serializable]
    public class ProjectileLifetime
    {
        [SerializeField, Min(0)]
        private float duration = 3f;

        [FormerlySerializedAs("onEnd")]
        [SerializeField]
        private EndBehaviour endBehaviour = EndBehaviour.Nothing;

        public float Duration => duration;
        public EndBehaviour OnEndBehaviour => endBehaviour;

        public enum EndBehaviour
        {
            Nothing = 0,
            Execute = 1,
        }
    }
}