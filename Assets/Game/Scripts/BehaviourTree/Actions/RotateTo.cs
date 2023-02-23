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
            Transform agentTransform = agent.transform;
            
            Vector3 lookDirection = target.value.transform.position - agentTransform.position;
            if ( Vector3.Angle(lookDirection, agentTransform.forward) <= angleDifference.value ) {
                EndAction();
                return;
            }

            agent.LookTo(lookDirection);
            
            if (!waitActionFinish) {
                EndAction();
            }
        }
	}
}