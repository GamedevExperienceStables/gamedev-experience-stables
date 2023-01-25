using BehaviourTree;
using UnityEngine;

namespace Game.BehaviourTree.Decorators
{
    [System.Serializable]
    public class Timeout : DecoratorNode
    {
        public float duration = 1.0f;
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
            if (Time.time - startTime > duration)
            {
                return State.Failure;
            }

            return child.Update();
        }
    }
}