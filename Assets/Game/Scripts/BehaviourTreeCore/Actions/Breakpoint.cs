using UnityEngine;

namespace BehaviourTreeCore
{
    [System.Serializable]
    public class Breakpoint : ActionNode
    {
        protected override void OnStart()
        {
            Debug.Log("Trigging Breakpoint");
            Debug.Break();
        }

        protected override void OnStop() { }

        protected override State OnUpdate() => State.Success;
    }
}
