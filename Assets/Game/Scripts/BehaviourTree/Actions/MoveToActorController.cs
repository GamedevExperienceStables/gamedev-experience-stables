using Game.Enemies;
using Game.Utils;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace Game.BehaviourTree.Actions
{
    [Name("Move To (Transform)")]
    [Category("Movement/Pathfinding")]
    public class MoveToActorController : ActionTask<NavigationController>
    {
        [RequiredField]
        public BBParameter<Transform> target;

        private Vector3 _lastRequest;

        protected override string info => "Move to " + target;

        protected override void OnExecute()
        {
            if (agent.IsReached(target.value.transform.position))
                EndAction(true);
        }

        protected override void OnUpdate()
        {
            Vector3 targetPosition = target.value.transform.position;
            if (!_lastRequest.AlmostEquals(targetPosition) && !agent.SetDestination(targetPosition))
            {
                EndAction(false);
                return;
            }

            agent.Tick();

            _lastRequest = targetPosition;

            if (agent.IsCompleted)
                EndAction(true);
        }

        protected override void OnPause()
            => OnStop();

        protected override void OnStop()
        {
            agent.Stop();

            _lastRequest = default;
        }
    }
}