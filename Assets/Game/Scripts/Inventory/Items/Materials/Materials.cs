using System.Collections.Generic;
using Game.Settings;
using UnityEngine;

namespace Game.Inventory
{
    public class Materials : IReadOnlyMaterials
    {
        private readonly MaterialContainer _bag = new(MaterialContainerId.Bag);
        private readonly MaterialContainer _container = new(MaterialContainerId.RocketContainer);

        public Materials(List<LevelDefinition> levels, int bagStack)
        {
            foreach (LevelDefinition level in levels)
            {
                LevelGoalSettings goal = level.Goal;

                _container.Create(goal.Material, goal.Count, 0);
                _bag.Create(goal.Material, bagStack, 0);
            }
        }

        public IReadOnlyMaterialContainer Container => _container;
        public IReadOnlyMaterialContainer Bag => _bag;

        public void Init()
        {
            _container.Reset();
            ClearBag();
        }

        public void Init(IEnumerable<MaterialInitialData> container, IEnumerable<MaterialInitialData> bag)
        {
            foreach (MaterialInitialData data in container)
                _container.SetValue(data.material, data.quantity);
            
            foreach (MaterialInitialData data in bag)
                _bag.SetValue(data.material, data.quantity);
        }

        public void ClearBag() 
            => _bag.Reset();

        public bool CanAddToBag(MaterialDefinition definition)
            => !_bag.IsFull(definition);

        public void AddToBag(MaterialDefinition definition)
        {
            if (_bag.IsFull(definition))
                return;

            _bag.Increase(definition);
        }

        public bool CanTransferToContainer(MaterialDefinition definition)
            => !_container.IsFull(definition) && !_bag.IsEmpty(definition);

        public void TransferToContainer(MaterialDefinition definition)
        {
            int containerCurrent = _container.GetCurrentValue(definition);
            int containerTotal = _container.GetTotalValue(definition);
            int bagCurrent = _bag.GetCurrentValue(definition);

            int transferValue = Mathf.Min(bagCurrent, containerTotal - containerCurrent);
            _bag.SetValue(definition, bagCurrent - transferValue);
            _container.SetValue(definition, containerCurrent + transferValue);
        }
    }
}