using Game.Enemies;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;


namespace Game.BehaviourTree.Conditions{
    [Name("Is on (Transform)")]
	[Category("Movement/Pathfinding")]
	public class IsOnTarget : ConditionTask<EnemyController>
    {
        public BBParameter<Transform> target;
        public BBParameter<float> stopDistance = 2f;

        protected override string info
            => "On " + $"{target}";
        
		protected override bool OnCheck(){
			return Vector3.Distance(agent.transform.position, target.value.position) <= stopDistance.value;
		}
	}
}