using System;
using System.Collections.Generic;

namespace Game.Inventory
{
    public interface IReadOnlyRecipes
    {
        void Subscribe(Action<RecipeDefinition> callback);
        void UnSubscribe(Action<RecipeDefinition> callback);
        
        bool Contains(RecipeDefinition recipe);

        IReadOnlyList<RecipeDefinition> Items { get; }
    }
}