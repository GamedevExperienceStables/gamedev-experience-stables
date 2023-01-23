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
            switch (interactable)
            {
                case LocationDoor locationDoor:
                {
                    var teleport = _resolver.Resolve<TransitionToLocationInteraction>();
                    teleport.Init(locationDoor.TargetLocation, interactable.gameObject);
                    return teleport;
                }
                
                case LootItem item:
                {
                    var pickup = _resolver.Resolve<ItemPickupInteraction>();
                    pickup.Init(item.Definition, instigator, interactable.gameObject);
                    return pickup;
                }
                
                default:
                    return null;
            }
        }
    }
}