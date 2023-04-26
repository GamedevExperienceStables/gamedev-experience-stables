using System;
using Game.Actors;
using VContainer;

namespace Game.Level
{
    public class InteractionFactory
    {
        private readonly IObjectResolver _resolver;

        [Inject]
        public InteractionFactory(IObjectResolver resolver) 
            => _resolver = resolver;

        public Interaction Create(Interactable interactable, IActorController instigator)
        {
            Type interactionType = InteractionBindingMap.GetInteractionType(interactable);
            var interaction = (Interaction)_resolver.Resolve(interactionType);
            interaction.Source = interactable.gameObject;
            interaction.Instigator = instigator;
            interaction.OnCreate();
            return interaction;
        }
    }
}