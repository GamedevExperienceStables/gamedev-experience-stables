using System.Collections.Generic;
using System.Linq;
using Game.Level;
using UnityEngine;

namespace Game.Actors
{
    public class InteractionController : MonoBehaviour
    {
        [SerializeField]
        private ZoneTrigger trigger;

        private readonly List<Interaction> _potentialInteractions = new();

        private InteractionService _interactionService;
        private Interactable _activeInteraction;

        public void Init(InteractionService interactionService) 
            => _interactionService = interactionService;

        private void OnEnable()
        {
            trigger.TriggerEntered += OnTriggerEntered;
            trigger.TriggerExited += OnTriggerExited;
        }

        private void OnDisable()
        {
            trigger.TriggerEntered -= OnTriggerEntered;
            trigger.TriggerExited -= OnTriggerExited;
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

            Interaction interaction = _interactionService.CreateInteraction(interactable, this);
            _potentialInteractions.Add(interaction);

            _interactionService.SetInteraction(interaction);
        }

        private void RemoveInteraction(Object source)
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

        public void Interact()
        {
            if (_potentialInteractions.Count == 0)
                return;

            Interaction interaction = _potentialInteractions.First();
            _interactionService.StartInteraction(interaction);
        }

        private bool InteractionExists(Object source)
            => _potentialInteractions.Exists(interaction => ReferenceEquals(interaction.Source, source));

        private Interaction FindInteraction(Object source)
            => _potentialInteractions.Find(interaction => ReferenceEquals(interaction.Source, source));
    }
}