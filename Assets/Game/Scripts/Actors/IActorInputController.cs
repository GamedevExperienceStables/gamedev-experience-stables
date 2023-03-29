using UnityEngine;

namespace Game.Actors
{
    public interface IActorInputController
    {
        Vector3 DesiredDirection { get; }
        void BlockInput(bool isBlocked);
        Vector3 GetTargetPosition(bool grounded = false);
    }
}