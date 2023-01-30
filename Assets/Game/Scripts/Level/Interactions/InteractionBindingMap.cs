using System;
using System.Collections.Generic;

namespace Game.Level
{
    public static class InteractionBindingMap
    {
        private static readonly Dictionary<Type, Type> Map = new()
        {
            [typeof(LocationDoor)] = typeof(TransitionToLocationInteraction),
            [typeof(RocketContainer)] = typeof(RocketContainerInteraction),
        };

        public static Type GetInteractionType(Interactable interactable)
            => Map[interactable.GetType()];
    }
}