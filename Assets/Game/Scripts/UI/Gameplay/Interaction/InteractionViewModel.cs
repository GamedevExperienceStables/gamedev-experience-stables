using System;
using Game.Level;
using VContainer;

namespace Game.UI
{
    public class InteractionViewModel
    {
        private readonly InteractionService _interaction;

        [Inject]
        public InteractionViewModel(InteractionService interaction) 
            => _interaction = interaction;

        public Interaction CurrentInteraction => _interaction.CurrentInteraction;

        public void SubscribeEnabled(Action<Interaction> callback) 
            => _interaction.Enabled += callback;

        public void UnSubscribeEnabled(Action<Interaction> callback) 
            => _interaction.Enabled -= callback;
        
        public void SubscribeDisabled(Action callback) 
            => _interaction.Disabled += callback;

        public void UnSubscribeDisabled(Action callback) 
            => _interaction.Disabled -= callback;
        
        public void SubscribeInteracted(Action<Interaction> callback)
            => _interaction.Interacted += callback;
        
        public void UnSubscribeInteracted(Action<Interaction> callback)
            => _interaction.Interacted -= callback;
    }
}