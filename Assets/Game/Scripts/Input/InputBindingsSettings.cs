using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Input
{
    [Serializable]
    public class InputBindingsSettings
    {
        [SerializeField]
        private List<InputKeyBinding> bindings;

        public List<InputKeyBinding> Bindings => bindings;
    }
}