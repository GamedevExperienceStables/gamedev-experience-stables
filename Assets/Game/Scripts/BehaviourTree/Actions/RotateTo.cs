using Game.Enemies;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;


namespace Game.BehaviourTree.Actions
{
	[Category("Movement/Pathfinding")]
	public class RotateTo : ActionTask<NavigationController>{
        
        [RequiredField]
        public BBParameter<Transform> target;
        public BBParameter<float> speed = 2;
        [SliderField(1, 180)]
        public BBParameter<float> angleDifference = 3;
        public bool waitActionFinish;
        
        protected override string info => "Rotate to " + target;

        protected override void OnUpdate() {
            if ( Vector3.Angle(target.value.transform.position - agent.transform.position, agent.transform.forward) <= angleDifference.value ) {
                EndAction();
                return;
            }
            // TODO: Fix rotate
            Transform transform = agent.transform;
            Vector3 dir = target.value.transform.position - transform.position;
            agent.LookTo(Vector3.RotateTowards(transform.forward, dir, speed.value * Time.deltaTime, 0));
            if ( !waitActionFinish ) {
                EndAction();
            }
        }
	}
}