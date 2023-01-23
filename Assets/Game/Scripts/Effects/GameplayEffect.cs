using Game.Actors;

namespace Game.Effects
{
    public abstract class GameplayEffect
    {
        public abstract void Execute(IActorController target, IActorController instigator);
    }
}