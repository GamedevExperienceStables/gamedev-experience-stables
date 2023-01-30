﻿using Game.Settings;
using UnityEngine;

namespace Game.Inventory
{
    public class Materials : IReadOnlyMaterials
    {
        private readonly MaterialContainer _bag = new(MaterialContainerId.Bag);
        private readonly MaterialContainer _container = new(MaterialContainerId.RocketContainer);

        public IReadOnlyMaterialContainer Container => _container;
        public IReadOnlyMaterialContainer Bag => _bag;

        public void CreateDefaults(LevelsSettings settings, int bagStack)
        {
            foreach (LevelDefinition level in settings.Levels)
            {
                LevelGoalSettings goal = level.Goal;

                _container.Create(goal.Material, goal.Count, 0);
                _bag.Create(goal.Material, bagStack, 0);
            }
        }

        public void Init()
        {
            _container.Reset();
            _bag.Reset();
        }

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