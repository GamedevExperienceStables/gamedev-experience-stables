﻿using System;
using UnityEngine;

namespace Game.Level
{
    [RequireComponent(typeof(BoxCollider))]
    public class LevelBoundary : MonoBehaviour, ILevelBoundary
    {
        private BoxCollider _collider;
        public Vector3 Center => _collider.center + transform.position;
        public Vector3 Size => _collider.size;

        private void Awake() 
            => _collider = GetComponent<BoxCollider>();
    }
}