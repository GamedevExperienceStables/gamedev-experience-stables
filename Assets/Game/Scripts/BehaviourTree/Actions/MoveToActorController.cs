using Game.Enemies;
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

        protected override string info => "Move to " + target;
        
        protected override void OnExecute()
        {
            if ( Vector3.Distance(agent.transform.position, target.value.transform.position) <= agent.StopDistance ) {
                EndAction(true);
            }
        }

        protected override void OnUpdate()
        {
            Vector3 pos = target.value.transform.position;
            agent.MoveToPosition(pos);
            if ( Vector3.Distance(agent.transform.position, target.value.transform.position) <= agent.StopDistance ) {
                EndAction(true);
            }
        }

        protected override void OnPause()
            => agent.Stop();

        protected override void OnStop()
            => agent.Stop();
    }
}