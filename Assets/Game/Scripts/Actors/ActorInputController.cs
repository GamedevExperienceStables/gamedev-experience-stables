using UnityEngine;

namespace Game.Actors
{
    public abstract class ActorInputController : MonoBehaviour, IActorInputController
    {
        protected readonly ActorBlock block = new();

        public abstract Vector3 DesiredDirection { get; }

        public bool HasAnyBlock(InputBlock input)
            => block.HasAny(input);

        public void SetBlock(bool isBlocked)
        {
            if (isBlocked)
                SetBlock(InputBlockExtensions.ALL);
            else
                RemoveBlock(InputBlockExtensions.ALL);
        }

        public virtual void SetBlock(InputBlock input)
            => block.SetBlock(input);

        public virtual void RemoveBlock(InputBlock input)
            => block.RemoveBlock(input);

        public abstract Vector3 GetTargetPosition(bool grounded = false);
    }
}