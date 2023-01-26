using Game.Actors;
using VContainer;

namespace Game.Effects
{
    public abstract class GameplayEffect
    {
    }
    
    public abstract class GameplayEffect<TDefinition> : GameplayEffect
    {
        protected TDefinition definition;

        public void Init(TDefinition definition) 
            => this.definition = definition;


        public class Factory
        {
            private readonly IObjectResolver _resolver;

            [Inject]
            public Factory(IObjectResolver resolver) 
                => _resolver = resolver;

            public TDefinition Create() 
                => _resolver.Resolve<TDefinition>();
        }

        public abstract void Execute(IActorController target, IActorController instigator);
    }
}