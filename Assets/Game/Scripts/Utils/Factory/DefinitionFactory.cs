using UnityEngine;

namespace Game.Utils.Factory
{
    public abstract class DefinitionFactory<T, TFactory> : ScriptableObject
    {
        public abstract T CreateRuntimeInstance(TFactory factory);
    }
}