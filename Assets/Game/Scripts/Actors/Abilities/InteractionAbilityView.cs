using System.Collections.Generic;
using System.Linq;
using Game.Level;
using UnityEngine;
using VContainer;

namespace Game.Actors
{
    public class InteractionAbilityView : ActorAbilityView
    {
        [SerializeField]
        private ZoneTrigger trigger;

        private readonly List<Interaction> _potentialInteractions = new();

        private InteractionService _interactionService;
        private Interactable _activeInteraction;

        [Inject]
        public void Construct(InteractionService interactionService)
        {
            _interactionService = interactionService;
        }

        public override bool CanActivateAbility() 
            => _potentialInteractions.Count > 0;

        protected override void OnEnableAbility()
        {
            trigger.TriggerEntered += OnTriggerEntered;
            trigger.TriggerExited += OnTriggerExited;
        }

        protected override void OnDisableAbility()
        {
            trigger.TriggerEntered -= OnTriggerEntered;
            trigger.TriggerExited -= OnTriggerExited;
        }

        protected override void OnResetAbility()
        {
            _potentialInteractions.Clear();
            _interactionService.ReleaseInteraction();
        }

        private void OnTriggerEntered(GameObject other)
        {
            if (!other.TryGetComponent(out Interactable interactable))
                return;

            if (InteractionExists(other))
                return;

            AddInteraction(interactable);
        }

        private void OnTriggerExited(GameObject other)
        {
            if (!other.TryGetComponent(out Interactable interactable))
                return;
            
            if (!InteractionExists(other))
                return;

            RemoveInteraction(interactable.gameObject);
        }

        private void AddInteraction(Interactable interactable)
        {
            Debug.Log($"[INTERACTION] +{interactable.gameObject.name}");

            Interaction interaction = _interactionService.CreateInteraction(interactable, Owner);
            _potentialInteractions.Add(interaction);

            _interactionService.SetInteraction(interaction);
        }

        private void RemoveInteraction(GameObject source)
        {
            Interaction existsInteraction = FindInteraction(source);
            _potentialInteractions.Remove(existsInteraction);

            if (_potentialInteractions.Count > 0)
            {
                Interaction nextInteraction = _potentialInteractions.First();
                _interactionService.SetInteraction(nextInteraction);
            }
            else
            {
                _interactionService.ReleaseInteraction();
            }
        }

        protected override void OnActivateAbility()
        {
            Interaction interaction = _potentialInteractions.First();
            _interactionService.StartInteraction(interaction);
        }

        private bool InteractionExists(Object source)
            => _potentialInteractions.Exists(interaction => ReferenceEquals(interaction.Source, source));

        private Interaction FindInteraction(Object source)
            => _potentialInteractions.Find(interaction => ReferenceEquals(interaction.Source, source));
    }
}