using System.Collections.Generic;
using UnityEngine;

namespace Game.Inventory
{
    public class MaterialContainer : IReadOnlyMaterialContainer
    {
        public delegate void MaterialChangedEvent(MaterialChangedData change);

        private readonly Dictionary<MaterialDefinition, MaterialData> _container = new();
        private event MaterialChangedEvent ValueChanged;

        public MaterialContainer(MaterialContainerId id)
            => Id = id;

        public MaterialContainerId Id { get; }

        public void Subscribe(MaterialChangedEvent callback)
            => ValueChanged += callback;

        public void UnSubscribe(MaterialChangedEvent callback)
            => ValueChanged -= callback;

        public void Create(MaterialDefinition material, int total, int current)
            => _container.Add(material, new MaterialData(material, total, current));

        public void Reset()
        {
            foreach (MaterialData material in _container.Values)
                SetValue(material.Definition, 0);
        }

        public int GetCurrentValue(MaterialDefinition definition)
            => _container[definition].Current;

        public int GetTotalValue(MaterialDefinition definition)
            => _container[definition].Total;

        public void Increase(MaterialDefinition definition)
            => SetValue(definition, _container[definition].Current + 1);

        public void SetValue(MaterialDefinition definition, int newValue)
        {
            MaterialData material = _container[definition];

            newValue = Mathf.Clamp(newValue, 0, material.Total);

            if (material.Current == newValue)
                return;

            int oldValue = material.Current;
            material.Current = newValue;

            Debug.Log($"[MATERIAL] {Id} | {material.Definition.name}: {oldValue} -> {material.Current}");
            ValueChanged?.Invoke(new MaterialChangedData(material.Definition, oldValue, material.Current));
        }

        public bool IsFull(MaterialDefinition definition)
        {
            MaterialData material = _container[definition];
            return material.Current >= material.Total;
        }

        public bool IsEmpty(MaterialDefinition definition)
            => _container[definition].Current <= 0;
    }
}