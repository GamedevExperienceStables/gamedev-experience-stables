using Game.Actors;
using Game.Stats;
using NodeCanvas.Framework;
using ParadoxNotion;
using ParadoxNotion.Design;


namespace Game.BehaviourTree.Conditions
{
    [Category("System Events")]
    public class CheckHealth : ConditionTask<ActorController>
    {
        [SliderField(1, 100)]
        public BBParameter<float> healthPercents = 100;
        
        public CompareMethod checkType = CompareMethod.GreaterOrEqualTo;
        
        protected override string info 
            => "Current health" + OperationTools.GetCompareString(checkType) + healthPercents + "% of 'Health'";

        protected override bool OnCheck()
        {
            float currentHealthPercents = agent.GetCurrentValue(CharacterStats.Health) /
                agent.GetCurrentValue(CharacterStats.HealthMax) * 100;
            return OperationTools.Compare(healthPercents.value, currentHealthPercents, checkType, 0);
        }
    }
}