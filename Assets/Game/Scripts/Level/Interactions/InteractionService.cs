using Game.Actors;
using VContainer;

namespace Game.Level
{
    public class InteractionService
    {
        private readonly InteractionFactory _factory;
        
        private Interaction _currentInteraction;

        [Inject]
        public InteractionService(InteractionFactory factory)
            => _factory = factory;

        public Interaction CreateInteraction(Interactable interactable, IActorController instigator) 
            => _factory.Create(interactable, instigator);

        public void SetInteraction(Interaction interaction)
        {
            _currentInteraction = interaction;
            
            // Show UI 
            // ...
        }

        public void ReleaseInteraction()
        {
            _currentInteraction = null;
            
            // Hide UI
            //...
        }

        public void StartInteraction(Interaction interaction)
        {
            ReleaseInteraction();
            
            interaction.Execute();
        }
    }
}