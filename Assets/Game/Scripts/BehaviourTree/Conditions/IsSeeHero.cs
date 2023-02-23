using Game.Enemies;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace Game.BehaviourTree.Conditions
{
    [Category("Movement/Pathfinding")]
    public class IsSeeHero : ConditionTask<AiSensor>
    {
        public BBParameter<Transform> target;

        protected override bool OnCheck()
        {
            if (target.isNoneOrNull)
                return false;
            
            return agent.Scan(target.value);
        }
    }
}