using System;
using Game.Inventory;
using UnityEngine;

namespace Game.Level
{
    [DisallowMultipleComponent]
    public class RocketContainer : Interactable
    {
        [SerializeField]
        private MaterialDefinition targetMaterial;

        public MaterialDefinition TargetMaterial => targetMaterial;

        public event Action TransferCompleted;

        public void TransferComplete()
            => TransferCompleted?.Invoke();
    }
}