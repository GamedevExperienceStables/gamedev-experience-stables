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

        public Interaction Create(Interactable interactable, InteractionController instigator)
        {
            switch (interactable)
            {
                case LocationDoor locationDoor:
                    {
                        var teleport = _resolver.Resolve<TransitionToLocationInteraction>();
                        teleport.Init(interactable.gameObject, locationDoor.TargetLocation);
                        return teleport;
                    }
                default:
                    return null;
            }
        }
    }
}