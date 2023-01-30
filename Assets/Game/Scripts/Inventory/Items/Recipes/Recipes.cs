using System;
using System.Collections.Generic;

namespace Game.Inventory
{
    public class Recipes : IReadOnlyRecipes
    {
        private readonly List<RecipeDefinition> _items = new();

        private event Action<RecipeDefinition> RecipeAdded;

        public void Subscribe(Action<RecipeDefinition> callback)
            => RecipeAdded += callback;

        public void UnSubscribe(Action<RecipeDefinition> callback)
            => RecipeAdded -= callback;

        public void Init()
            => _items.Clear();

        public bool Contains(RecipeDefinition recipe)
            => _items.Contains(recipe);

        public void Add(RecipeDefinition recipe)
        {
            _items.Add(recipe);

            RecipeAdded?.Invoke(recipe);
        }
    }
}