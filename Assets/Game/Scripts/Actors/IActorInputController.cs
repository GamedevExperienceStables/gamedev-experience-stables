using UnityEngine;

namespace Game.Actors
{
    public interface IActorInputController
    {
        void BlockInput(bool isBlocked);
        Vector3 DesiredDirection { get; }
    }
}