using System;

namespace Game.Inventory
{
    public interface IReadOnlyRecipes
    {
        void Subscribe(Action<RecipeDefinition> callback);
        void UnSubscribe(Action<RecipeDefinition> callback);
        
        bool Contains(RecipeDefinition recipe);
    }
}