using BehaviourTree;

namespace Game.BehaviourTree.Decorators
{
    [System.Serializable]
    public class Failure : DecoratorNode
    {
        protected override void OnStart()
        {
        }

        protected override void OnStop()
        {
        }

        protected override State OnUpdate()
        {
            var state = child.Update();
            if (state == State.Success)
            {
                return State.Failure;
            }
            return state;
        }
    }
}