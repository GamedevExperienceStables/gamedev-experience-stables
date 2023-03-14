using System;
using Game.Actors;
using VContainer;

namespace Game.Level
{
    public class InteractionService
    {
        private readonly InteractionFactory _factory;

        public event Action<Interaction> Enabled;
        public event Action Disabled;

        [Inject]
        public InteractionService(InteractionFactory factory)
            => _factory = factory;

        public Interaction CreateInteraction(Interactable interactable, IActorController instigator) 
            => _factory.Create(interactable, instigator);

        public void SetInteraction(Interaction interaction) 
            => Enabled?.Invoke(interaction);

        public void ReleaseInteraction() 
            => Disabled?.Invoke();

        public void StartInteraction(Interaction interaction)
        {
            ReleaseInteraction();
            
            interaction.Execute();
        }
    }
}