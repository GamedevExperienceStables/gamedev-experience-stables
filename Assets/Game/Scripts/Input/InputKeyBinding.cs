using System;
using UnityEngine;

namespace Game.Input
{
    [Serializable]
    public struct InputKeyBinding
    {
        [SerializeField]
        private string key;
        
        [SerializeField]
        private Sprite icon;

        public InputKeyBinding(string key)
        {
            this.key = key;
            icon = null;
        }

        public readonly Sprite Icon => icon;

        public readonly string Key => key;
    }
}