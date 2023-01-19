namespace BehaviourTreeCore
{
    [System.Serializable]
    public class Succeed : DecoratorNode
    {
        protected override void OnStart() { }

        protected override void OnStop() { }

        protected override State OnUpdate()
        {
            var state = child.Update();
            if (state == State.Failure)
            {
                return State.Success;
            }
            return state;
        }
    }
}