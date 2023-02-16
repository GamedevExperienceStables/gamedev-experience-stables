using System;
using System.Collections.Generic;

namespace Game.Inventory
{
    public class Recipes : IReadOnlyRecipes
    {
        private readonly List<RecipeDefinition> _items = new();
        
        private event Action<RecipeDefinition> RecipeAdded;
        
        public IReadOnlyList<RecipeDefinition> Items => _items.AsReadOnly();

        public void Subscribe(Action<RecipeDefinition> callback)
            => RecipeAdded += callback;

        public void UnSubscribe(Action<RecipeDefinition> callback)
            => RecipeAdded -= callback;

        public void Reset()
            => _items.Clear();
        
        public void Init(IEnumerable<RecipeDefinition> recipes)
        {
            _items.Clear();
            
            foreach (RecipeDefinition recipe in recipes)
                _items.Add(recipe);
        }

        public bool Contains(RecipeDefinition recipe)
            => _items.Contains(recipe);

        public void Add(RecipeDefinition recipe)
        {
            _items.Add(recipe);

            RecipeAdded?.Invoke(recipe);
        }
    }
}