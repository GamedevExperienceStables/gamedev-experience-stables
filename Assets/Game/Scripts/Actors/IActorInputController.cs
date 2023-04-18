using UnityEngine;

namespace Game.Actors
{
    public interface IActorInputController
    {
        Vector3 DesiredDirection { get; }
        
        bool HasAnyBlock(InputBlock input);
        void SetBlock(bool isBlocked);
        void SetBlock(InputBlock input);
        void RemoveBlock(InputBlock input);
        
        Vector3 GetTargetPosition(bool grounded = false);
    }
}