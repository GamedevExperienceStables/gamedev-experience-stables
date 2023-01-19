using BehaviourTree;
using UnityEngine;

namespace Game.BehaviourTree.Actions
{

    [System.Serializable]
    public class Wait : ActionNode
    {

        public float duration = 1;
        float startTime;

        protected override void OnStart()
        {
            startTime = Time.time;
        }

        protected override void OnStop()
        {
        }

        protected override State OnUpdate()
        {

            float timeRemaining = Time.time - startTime;
            if (timeRemaining > duration)
            {
                return State.Success;
            }
            return State.Running;
        }
    }
}
