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

        public IReadOnlyMaterialData GetMaterialData(MaterialDefinition definition)
            => _container[definition];

        public void Create(MaterialDefinition material, int total, int current)
            => _container.Add(material, new MaterialData(material, total, current));

        public void Reset()
        {
            foreach (MaterialData material in _container.Values)
                SetValue(material.Definition, 0);
        }

        public int GetCurrentValue(MaterialDefinition definition) 
            => _container.TryGetValue(definition, out MaterialData material) ? material.Current : 0;

        public int GetTotalValue(MaterialDefinition definition)
            => _container.TryGetValue(definition, out MaterialData material) ? material.Total : 0;

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
            if (_container.TryGetValue(definition, out MaterialData material))
                return material.Current >= material.Total;

            return false;
        }

        public bool IsEmpty(MaterialDefinition definition)
        {
            if (_container.TryGetValue(definition, out MaterialData material))
                return material.Current <= 0;

            return true;
        }
    }
}